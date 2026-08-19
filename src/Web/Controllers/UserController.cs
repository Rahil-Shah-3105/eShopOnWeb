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
		_logger.LogDebug("UserController(tokenClaimsService=<redacted>,signInManager={SignInManager},logger={Logger},cache={Cache}): _tokenClaimsService → {_tokenClaimsService}", signInManager, logger, cache, _tokenClaimsService);
        _signInManager = signInManager;
		_logger.LogDebug("UserController(tokenClaimsService=<redacted>,signInManager={SignInManager},logger={Logger},cache={Cache}): _signInManager → {_signInManager}", signInManager, logger, cache, _signInManager);
        _logger = logger;
		_logger.LogDebug(">>> Entering UserController(tokenClaimsService=<redacted>,signInManager={SignInManager},logger={Logger},cache={Cache})", signInManager, logger, cache);
		_logger.LogDebug("UserController(tokenClaimsService=<redacted>,signInManager={SignInManager},logger={Logger},cache={Cache}): _logger → {_logger}", signInManager, logger, cache, _logger);
        _cache = cache;
		_logger.LogDebug("UserController(tokenClaimsService=<redacted>,signInManager={SignInManager},logger={Logger},cache={Cache}): _cache → {_cache}", signInManager, logger, cache, _cache);
		_logger.LogDebug("<<< Exiting UserController(tokenClaimsService=<redacted>,signInManager={SignInManager},logger={Logger},cache={Cache})", signInManager, logger, cache);
    }

    [HttpGet]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> GetCurrentUser() =>
        Ok(await CreateUserInfo(User));
		_logger.LogDebug(">>> Entering GetCurrentUser()");

    [Route("Logout")]
    [HttpPost]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
		_logger.LogDebug(">>> Entering Logout()");
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var userId = _signInManager.Context.User.Claims.First(c => c.Type == ClaimTypes.Name);
        var identityKey = _signInManager.Context.Request.Cookies[ConfigureCookieSettings.IdentifierCookieName];
		_logger.LogDebug("Logout(): userId → {UserId}", userId);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        _cache.Set($"{userId.Value}:{identityKey}", identityKey, new MemoryCacheEntryOptions
        {
			_logger.LogInformation("Logout(): database query executed in {Arg0} ms", __logStart.ElapsedMilliseconds);
            AbsoluteExpiration = DateTime.Now.AddMinutes(ConfigureCookieSettings.ValidityMinutesPeriod)
        });

        _logger.LogInformation("User logged out.");
		_logger.LogDebug("<<< Exiting Logout()");
        return Ok();
    }

    private async Task<UserInfo> CreateUserInfo(ClaimsPrincipal claimsPrincipal)
    {
		_logger.LogDebug(">>> Entering CreateUserInfo(claimsPrincipal={ClaimsPrincipal})", claimsPrincipal);
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
			_logger.LogDebug("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): userInfo.NameClaimType → {Arg1}", claimsPrincipal, userInfo.NameClaimType);
			_logger.LogDebug("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): userInfo.RoleClaimType → {Arg1}", claimsPrincipal, userInfo.RoleClaimType);
            userInfo.RoleClaimType = claimsIdentity.RoleClaimType;
        }
        else
        {
            userInfo.NameClaimType = "name";
            userInfo.RoleClaimType = "role";
        }

        if (claimsPrincipal.Claims.Any())
        {
            var claims = new List<ClaimValue>();
			_logger.LogDebug("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): claims → {Claims}", claimsPrincipal, claims);
            var nameClaims = claimsPrincipal.FindAll(userInfo.NameClaimType);
			_logger.LogDebug("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): nameClaims → {NameClaims}", claimsPrincipal, nameClaims);
            foreach (var claim in nameClaims)
            {
                claims.Add(new ClaimValue(userInfo.NameClaimType, claim.Value));
            }

            foreach (var claim in claimsPrincipal.Claims.Except(nameClaims))
            {
                claims.Add(new ClaimValue(claim.Type, claim.Value));
            }

            userInfo.Claims = claims;
			_logger.LogDebug("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): userInfo.Claims → {Arg1}", claimsPrincipal, userInfo.Claims);
        }

        var token = await _tokenClaimsService.GetTokenAsync(claimsPrincipal.Identity.Name);
		_logger.LogWarning("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): token → {Token}", claimsPrincipal, token);
		_logger.LogDebug("CreateUserInfo(claimsPrincipal={ClaimsPrincipal}): userInfo.Token → {Arg1}", claimsPrincipal, userInfo.Token);
        userInfo.Token = token;

		_logger.LogDebug("<<< Exiting CreateUserInfo(claimsPrincipal={ClaimsPrincipal})", claimsPrincipal);
        return userInfo;
    }
}
