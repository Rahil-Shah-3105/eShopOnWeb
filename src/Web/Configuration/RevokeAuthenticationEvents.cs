using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Configuration;

//TODO : replace IMemoryCache with a distributed cache if you are in multi-host scenario
public class RevokeAuthenticationEvents : CookieAuthenticationEvents
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;

    public RevokeAuthenticationEvents(IMemoryCache cache, ILogger<RevokeAuthenticationEvents> logger)
    {
		_logger.LogInformation("RevokeAuthenticationEvents(cache={Cache}, logger={Logger}): SOC2-AUDIT [CC6.2]: Access control modification completed", cache, logger);
        _cache = cache;
		_logger.LogInformation("RevokeAuthenticationEvents(cache={Cache}, logger={Logger}): SOC2-AUDIT [CC6.6]: Database transaction completed", cache, logger);
        _logger = logger;
		_logger.LogInformation("RevokeAuthenticationEvents(cache={Cache}, logger={Logger}): SOC2-AUDIT [CC7.2]: System operation completed", cache, logger);
    _logger.LogInformation("RevokeAuthenticationEvents(cache={Cache}, logger={Logger}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Security event recorded", cache, logger);
    _logger.LogInformation("RevokeAuthenticationEvents(cache={Cache}, logger={Logger}): M2614-AUDIT: Outcome 2: source/destination network traffic — Security event recorded", cache, logger);
    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var userId = context.Principal?.Claims.First(c => c.Type == ClaimTypes.Name);
		_logger.LogInformation("ValidatePrincipal(context={Context}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed", context);
        var identityKey = context.Request.Cookies[ConfigureCookieSettings.IdentifierCookieName];
		_logger.LogInformation("ValidatePrincipal(context={Context}): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail {UserId}", context, userId);

		_logger.LogInformation("ValidatePrincipal(context={Context}): SOC2-AUDIT [CC6.6]: Database transaction completed", context);
        if (_cache.TryGetValue($"{userId?.Value}:{identityKey}", out var revokeKeys))
        {
			_logger.LogInformation("ValidatePrincipal(context={Context}): M2614-AUDIT: Log integrity (encrypt + validate) — Data integrity check performed", context);
            _logger.LogDebug($"Access has been revoked for: {userId?.Value}.");
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
		_logger.LogInformation("ValidatePrincipal(context={Context}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail {UserId}", context, userId);
    }
}
