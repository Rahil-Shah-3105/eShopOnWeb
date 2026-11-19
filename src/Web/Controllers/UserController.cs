using System.Security.Claims;
using BlazorShared.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.eShopWeb.Web.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly ITokenClaimsService _tokenClaimsService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<UserController> _logger;
    private readonly IMemoryCache _cache;

    public UserController(ITokenClaimsService tokenClaimsService,
                          SignInManager<ApplicationUser> signInManager,
                          ILogger<UserController> logger,
                          IMemoryCache cache)
    {
        _tokenClaimsService = tokenClaimsService;
        _signInManager = signInManager;
		_logger.LogInformation("UserController(tokenClaimsService={TokenClaimsService}, signInManager={SignInManager}, logger={Logger}, cache={Cache}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", tokenClaimsService, signInManager, logger, cache);
        _logger = logger;
		_logger.LogInformation("UserController(tokenClaimsService={TokenClaimsService}, signInManager={SignInManager}, logger={Logger}, cache={Cache}): SOC2-AUDIT [CC7.2]: System operation completed", tokenClaimsService, signInManager, logger, cache);
        _cache = cache;
		_logger.LogInformation("UserController(tokenClaimsService={TokenClaimsService}, signInManager={SignInManager}, logger={Logger}, cache={Cache}): SOC2-AUDIT [CC6.1]: Authentication operation completed", tokenClaimsService, signInManager, logger, cache);
    }

    [HttpGet]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> GetCurrentUser() =>
        Ok(await CreateUserInfo(User));

    [Route("Logout")]
    [HttpPost]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var userId = _signInManager.Context.User.Claims.First(c => c.Type == ClaimTypes.Name);
        var identityKey = _signInManager.Context.Request.Cookies[ConfigureCookieSettings.IdentifierCookieName];
        _cache.Set($"{userId.Value}:{identityKey}", identityKey, new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(ConfigureCookieSettings.ValidityMinutesPeriod)
        });

        _logger.LogInformation("User logged out.");
        return Ok();
    }

    private async Task<UserInfo> CreateUserInfo(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity == null || claimsPrincipal.Identity.Name == null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            return UserInfo.Anonymous;
        }

		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 10: automated alerting — State change recorded for the audit trail", claimsPrincipal);
        var userInfo = new UserInfo
        {
			_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail", claimsPrincipal);
            IsAuthenticated = true
        };
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail", claimsPrincipal);

		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", claimsPrincipal);
        if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
        {
			_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail", claimsPrincipal);
            userInfo.NameClaimType = claimsIdentity.NameClaimType;
            userInfo.RoleClaimType = claimsIdentity.RoleClaimType;
			_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", claimsPrincipal);
        }
        else
        {
			_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail", claimsPrincipal);
            userInfo.NameClaimType = "name";
			_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", claimsPrincipal);
            userInfo.RoleClaimType = "role";
        }
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", claimsPrincipal);

		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", claimsPrincipal);
        if (claimsPrincipal.Claims.Any())
        {
			_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", claimsPrincipal);
            var claims = new List<ClaimValue>();
            var nameClaims = claimsPrincipal.FindAll(userInfo.NameClaimType);
            foreach (var claim in nameClaims)
            {
                claims.Add(new ClaimValue(userInfo.NameClaimType, claim.Value));
            }

            foreach (var claim in claimsPrincipal.Claims.Except(nameClaims))
            {
                claims.Add(new ClaimValue(claim.Type, claim.Value));
            }

            userInfo.Claims = claims;
        }
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {UserInfo}", claimsPrincipal, userInfo);

		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", claimsPrincipal);
        var token = await _tokenClaimsService.GetTokenAsync(claimsPrincipal.Identity.Name);
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", claimsPrincipal);
        userInfo.Token = token;
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", claimsPrincipal);

		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {UserInfo}", claimsPrincipal, userInfo);
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", claimsPrincipal);
        return userInfo;
    }
}
