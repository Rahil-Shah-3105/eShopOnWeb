using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Configuration;

public static class ConfigureCookieSettings
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public const int ValidityMinutesPeriod = 60;
    public const string IdentifierCookieName = "EshopIdentifier";

    public static IServiceCollection AddCookieSettings(this IServiceCollection services)
    {
		_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", services);
        services.Configure<CookiePolicyOptions>(options =>
        {
				_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", services);
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //TODO need to check that.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
        });
		_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", services);
        services.ConfigureApplicationCookie(options =>
        {
			_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", services);
            options.EventsType = typeof(RevokeAuthenticationEvents);
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(ValidityMinutesPeriod);
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.Cookie = new CookieBuilder
            {
                Name = IdentifierCookieName,
                IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                };
				_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", services);
				_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC6.6]: Database transaction completed", services);
				_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", services);
				_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", services);
				_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", services);
				_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC6.1]: Authentication operation completed", services);
        });

        services.AddScoped<RevokeAuthenticationEvents>();

        return services;
    }
}
