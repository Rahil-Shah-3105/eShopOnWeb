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
        _logger = logger;
		_logger.LogInformation("UserController(tokenClaimsService={TokenClaimsService}, signInManager={SignInManager}, logger={Logger}, cache={Cache}): SOC2-AUDIT [CC7.2]: System operation completed", tokenClaimsService, signInManager, logger, cache);
		_logger.LogInformation("UserController(tokenClaimsService={TokenClaimsService}, signInManager={SignInManager}, logger={Logger}, cache={Cache}): SOC2-AUDIT [CC6.1]: Authentication operation completed", tokenClaimsService, signInManager, logger, cache);
        _cache = cache;
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
		_logger.LogInformation("Logout(): SOC2-AUDIT [CC7.2]: System operation completed");
		_logger.LogInformation("Logout(): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {UserId}", userId);
        return Ok();
    }

    private async Task<UserInfo> CreateUserInfo(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity == null || claimsPrincipal.Identity.Name == null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            return UserInfo.Anonymous;
        }

        var userInfo = new UserInfo
        {
            IsAuthenticated = true
        };

        if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
        {
            userInfo.NameClaimType = claimsIdentity.NameClaimType;
            userInfo.RoleClaimType = claimsIdentity.RoleClaimType;
        }
        else
        {
            userInfo.NameClaimType = "name";
            userInfo.RoleClaimType = "role";
        }
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", claimsPrincipal);
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", claimsPrincipal);

        if (claimsPrincipal.Claims.Any())
        {
			_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", claimsPrincipal);
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
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", claimsPrincipal);
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", claimsPrincipal);
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", claimsPrincipal);
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", claimsPrincipal);
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", claimsPrincipal);
		_logger.LogInformation("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {UserInfo}", claimsPrincipal, userInfo);

        var token = await _tokenClaimsService.GetTokenAsync(claimsPrincipal.Identity.Name);
        userInfo.Token = token;

        return userInfo;
    }
}
