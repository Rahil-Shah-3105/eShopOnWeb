using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account;

//TODO : replace IMemoryCache by distributed cache if you are in multi-host scenario
public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;
    private readonly IMemoryCache _cache;

    public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, IMemoryCache cache)
    {
        _signInManager = signInManager;
		_logger.LogDebug("LogoutModel(signInManager={SignInManager},logger={Logger},cache={Cache}): _signInManager → {_signInManager}", signInManager, logger, cache, _signInManager);
        _logger = logger;
		_logger.LogDebug(">>> Entering LogoutModel(signInManager={SignInManager},logger={Logger},cache={Cache})", signInManager, logger, cache);
		_logger.LogDebug("LogoutModel(signInManager={SignInManager},logger={Logger},cache={Cache}): _logger → {_logger}", signInManager, logger, cache, _logger);
        _cache = cache;
		_logger.LogDebug("LogoutModel(signInManager={SignInManager},logger={Logger},cache={Cache}): _cache → {_cache}", signInManager, logger, cache, _cache);
		_logger.LogDebug("<<< Exiting LogoutModel(signInManager={SignInManager},logger={Logger},cache={Cache})", signInManager, logger, cache);
    }

    public void OnGet()
    {
	_logger.LogDebug(">>> Entering OnGet()");
	_logger.LogDebug("<<< Exiting OnGet()");
    }

    public async Task<IActionResult> OnPost(string? returnUrl = null)
    {
		_logger.LogDebug(">>> Entering OnPost(returnUrl={ReturnUrl})", returnUrl);
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var userId = _signInManager.Context.User.Claims.First(c => c.Type == ClaimTypes.Name);
        var identityKey = _signInManager.Context.Request.Cookies[ConfigureCookieSettings.IdentifierCookieName];
		_logger.LogDebug("OnPost(returnUrl={ReturnUrl}): userId → {UserId}", returnUrl, userId);
		_logger.LogInformation("OnPost(returnUrl)={ReturnUrl}: database query executed in {Arg1} ms", returnUrl, __logStart.ElapsedMilliseconds);
        _cache.Set($"{userId.Value}:{identityKey}", identityKey, new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(ConfigureCookieSettings.ValidityMinutesPeriod)
        });

        _logger.LogInformation("User logged out.");
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
			_logger.LogDebug("<<< Exiting OnPost(returnUrl={ReturnUrl})", returnUrl);
            return RedirectToPage("/Index");
        }
    }
}
