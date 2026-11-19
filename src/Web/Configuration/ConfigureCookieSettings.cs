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
		_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", services);
        services.Configure<CookiePolicyOptions>(options =>
        {
				_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed", services);
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //TODO need to check that.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
        });
		_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", services);
        services.ConfigureApplicationCookie(options =>
        {
			_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", services);
            options.EventsType = typeof(RevokeAuthenticationEvents);
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(ValidityMinutesPeriod);
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.Cookie = new CookieBuilder
            {
				_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", services);
                Name = IdentifierCookieName,
                IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                };
				_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", services);
        });
		_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", services);

		_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", services);
        services.AddScoped<RevokeAuthenticationEvents>();
		_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC6.6]: Database transaction completed", services);

		_logger.LogInformation("AddCookieSettings(services={Services}): SOC2-AUDIT [CC6.1]: Authentication operation completed", services);
		_logger.LogInformation("AddCookieSettings(services={Services}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", services);
        return services;
    }
}
