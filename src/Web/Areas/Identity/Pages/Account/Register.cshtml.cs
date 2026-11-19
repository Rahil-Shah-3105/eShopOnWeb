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
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
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
		_logger.LogDebug("RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender=<redacted>): _userManager → {_userManager}", userManager, signInManager, logger, _userManager);
        _signInManager = signInManager;
		_logger.LogDebug("RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender=<redacted>): _signInManager → {_signInManager}", userManager, signInManager, logger, _signInManager);
        _logger = logger;
		_logger.LogError(">>> Entering RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender=<redacted>)", userManager, signInManager, logger);
		_logger.LogDebug("RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender=<redacted>): _logger → {_logger}", userManager, signInManager, logger, _logger);
    _logger.LogWarning("<<< Exiting RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender=<redacted>)", userManager, signInManager, logger);
        _emailSender = emailSender;
		_logger.LogDebug("RegisterModel(userManager={UserManager},signInManager={SignInManager},logger={Logger},emailSender=<redacted>): _emailSender → <redacted>", userManager, signInManager, logger);
    }

    [BindProperty]
    public required InputModel Input { get; set; }

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
    	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
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
		_logger.LogError(">>> Entering OnGet(returnUrl={ReturnUrl})", returnUrl);
        ReturnUrl = returnUrl;
		_logger.LogDebug("OnGet(returnUrl={ReturnUrl}): ReturnUrl → {ReturnUrl1}", returnUrl, ReturnUrl);
    _logger.LogWarning("<<< Exiting OnGet(returnUrl={ReturnUrl})", returnUrl);
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
		_logger.LogError(">>> Entering OnPostAsync(returnUrl={ReturnUrl})", returnUrl);
        returnUrl = returnUrl ?? Url.Content("~/");
		_logger.LogError("OnPostAsync(returnUrl={ReturnUrl}): returnUrl → {ReturnUrl1}", returnUrl, returnUrl);
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = Input?.Email, Email = Input?.Email };
            var result = await _userManager.CreateAsync(user, Input?.Password!);
			_logger.LogError("OnPostAsync(returnUrl={ReturnUrl}): result → {Result}", returnUrl, result);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				_logger.LogError("OnPostAsync(returnUrl={ReturnUrl}): code → {Code}", returnUrl, code);
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
		_logger.LogWarning("<<< Exiting OnPostAsync(returnUrl={ReturnUrl})", returnUrl);
        return Page();
    }
}
