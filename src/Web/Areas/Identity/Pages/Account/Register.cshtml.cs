using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IEmailSender _emailSender;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender)
    {
        _userManager = userManager;
		_logger.LogDebug("RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender={EmailSender}): _userManager → {_userManager}", userManager, signInManager, logger, emailSender, _userManager);
        _signInManager = signInManager;
		_logger.LogDebug("RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender={EmailSender}): _signInManager → {_signInManager}", userManager, signInManager, logger, emailSender, _signInManager);
        _logger = logger;
		_logger.LogDebug(">>> Entering RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender={EmailSender})", userManager, signInManager, logger, emailSender);
		_logger.LogDebug("RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender={EmailSender}): _logger → {_logger}", userManager, signInManager, logger, emailSender, _logger);
        _emailSender = emailSender;
    }

    [BindProperty]
    public required InputModel Input { get; set; }

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }

    public void OnGet(string? returnUrl = null)
    {
		_logger.LogDebug(">>> Entering OnGet(returnUrl={ReturnUrl})", returnUrl);
        ReturnUrl = returnUrl;
		_logger.LogDebug("OnGet(returnUrl={ReturnUrl}): ReturnUrl → {ReturnUrl1}", returnUrl, ReturnUrl);
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
		_logger.LogDebug(">>> Entering OnPostAsync(returnUrl={ReturnUrl})", returnUrl);
        returnUrl = returnUrl ?? Url.Content("~/");
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = Input?.Email, Email = Input?.Email };
            var result = await _userManager.CreateAsync(user, Input?.Password!);
			_logger.LogDebug("OnPostAsync(returnUrl={ReturnUrl}): result → {Result}", returnUrl, result);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				_logger.LogDebug("OnPostAsync(returnUrl={ReturnUrl}): code → {Code}", returnUrl, code);
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                Guard.Against.Null(callbackUrl, nameof(callbackUrl));
                await _emailSender.SendEmailAsync(Input!.Email!, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
		_logger.LogDebug("<<< Exiting OnPostAsync(returnUrl={ReturnUrl})", returnUrl);
        return Page();
    }
}
