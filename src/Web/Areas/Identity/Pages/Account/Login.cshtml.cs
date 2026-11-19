using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;

namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly IBasketService _basketService;

    public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IBasketService basketService)
    {
        _signInManager = signInManager;
		_logger.LogDebug("LoginModel(signInManager={SignInManager},logger={Logger},basketService={BasketService}): _signInManager → {_signInManager}", signInManager, logger, basketService, _signInManager);
        _logger = logger;
		_logger.LogError(">>> Entering LoginModel(signInManager={SignInManager},logger={Logger},basketService={BasketService})", signInManager, logger, basketService);
        _basketService = basketService;
		_logger.LogDebug("LoginModel(signInManager={SignInManager},logger={Logger},basketService={BasketService}): _basketService → {_basketService}", signInManager, logger, basketService, _basketService);
    }

    [BindProperty]
    public required InputModel Input { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public class InputModel
    {
    	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string? returnUrl = null)
    {
		_logger.LogError(">>> Entering OnGetAsync(returnUrl={ReturnUrl})", returnUrl);
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl = returnUrl ?? Url.Content("~/");
		_logger.LogError("OnGetAsync(returnUrl={ReturnUrl}): returnUrl → {ReturnUrl1}", returnUrl, returnUrl);

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

		_logger.LogWarning("<<< Exiting OnGetAsync(returnUrl={ReturnUrl})", returnUrl);
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		_logger.LogError("OnGetAsync(returnUrl={ReturnUrl}): ExternalLogins → {ExternalLogins}", returnUrl, ExternalLogins);
		_logger.LogDebug("OnGetAsync(returnUrl={ReturnUrl}): ExternalLogins → {ExternalLogins}", returnUrl, ExternalLogins);

        ReturnUrl = returnUrl;
		_logger.LogDebug("OnGetAsync(returnUrl={ReturnUrl}): ReturnUrl → {ReturnUrl1}", returnUrl, ReturnUrl);
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
		_logger.LogError(">>> Entering OnPostAsync(returnUrl={ReturnUrl})", returnUrl);
        returnUrl = returnUrl ?? Url.Content("~/");

		_logger.LogError("OnPostAsync(returnUrl={ReturnUrl}): returnUrl → {ReturnUrl1}", returnUrl, returnUrl);
        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
            var result = await _signInManager.PasswordSignInAsync(Input!.Email!, Input!.Password!, 
                false, true);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                await TransferAnonymousBasketToUserAsync(Input?.Email);
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input?.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        // If we got this far, something failed, redisplay form
		_logger.LogWarning("<<< Exiting OnPostAsync(returnUrl={ReturnUrl})", returnUrl);
        return Page();
    }

    private async Task TransferAnonymousBasketToUserAsync(string? userName)
    {
		_logger.LogError(">>> Entering TransferAnonymousBasketToUserAsync(userName={UserName})", userName);
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
            var anonymousId = Request.Cookies[Constants.BASKET_COOKIENAME];
            if (Guid.TryParse(anonymousId, out var _))
            {
                Guard.Against.NullOrEmpty(userName, nameof(userName));
                await _basketService.TransferBasketAsync(anonymousId, userName);
            }
            Response.Cookies.Delete(Constants.BASKET_COOKIENAME);
        }
		_logger.LogWarning("<<< Exiting TransferAnonymousBasketToUserAsync(userName={UserName})", userName);
    }
}
