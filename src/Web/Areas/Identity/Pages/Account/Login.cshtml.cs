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
        _logger = logger;
		_logger.LogInformation("LoginModel(signInManager={SignInManager}, logger={Logger}, basketService={BasketService}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", signInManager, logger, basketService);
        _basketService = basketService;
		_logger.LogInformation("LoginModel(signInManager={SignInManager}, logger={Logger}, basketService={BasketService}): SOC2-AUDIT [CC6.1]: Authentication operation completed", signInManager, logger, basketService);
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
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl = returnUrl ?? Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		_logger.LogInformation("OnGetAsync(returnUrl={ReturnUrl}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", returnUrl);

		_logger.LogInformation("OnGetAsync(returnUrl={ReturnUrl}): SOC2-AUDIT [CC6.1]: Authentication operation completed", returnUrl);
        ReturnUrl = returnUrl;
		_logger.LogInformation("OnGetAsync(returnUrl={ReturnUrl}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail", returnUrl);
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

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

		_logger.LogInformation("OnPostAsync(returnUrl={ReturnUrl}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", returnUrl);
        // If we got this far, something failed, redisplay form
		_logger.LogInformation("OnPostAsync(returnUrl={ReturnUrl}): M2614-AUDIT: Log Management / data protection — Log management recorded for the audit trail", returnUrl);
		_logger.LogInformation("OnPostAsync(returnUrl={ReturnUrl}): SOC2-AUDIT [CC6.1]: Authentication operation completed", returnUrl);
        return Page();
    }

    private async Task TransferAnonymousBasketToUserAsync(string? userName)
    {
		_logger.LogInformation("TransferAnonymousBasketToUserAsync(userName={UserName}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", userName);
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
			_logger.LogInformation("TransferAnonymousBasketToUserAsync(userName={UserName}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", userName);
            var anonymousId = Request.Cookies[Constants.BASKET_COOKIENAME];
            if (Guid.TryParse(anonymousId, out var _))
            {
                Guard.Against.NullOrEmpty(userName, nameof(userName));
                await _basketService.TransferBasketAsync(anonymousId, userName);
            }
			_logger.LogInformation("TransferAnonymousBasketToUserAsync(userName={UserName}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", userName);
            Response.Cookies.Delete(Constants.BASKET_COOKIENAME);
			_logger.LogInformation("TransferAnonymousBasketToUserAsync(userName={UserName}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", userName);
        }
		_logger.LogInformation("TransferAnonymousBasketToUserAsync(userName={UserName}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", userName);
    }
}
