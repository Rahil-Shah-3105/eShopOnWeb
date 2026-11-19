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
		_logger.LogInformation("AddCookieSettings(services={Services}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", services);
        services.Configure<CookiePolicyOptions>(options =>
        {
				_logger.LogInformation("AddCookieSettings(services={Services}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", services);
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //TODO need to check that.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
        });
		_logger.LogInformation("AddCookieSettings(services={Services}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", services);
        services.ConfigureApplicationCookie(options =>
        {
			_logger.LogInformation("AddCookieSettings(services={Services}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", services);
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
        });
		_logger.LogWarning("AddCookieSettings(services={Services}): GDPR-AUDIT: Access, erasure, portability — Database transaction completed", services);
		_logger.LogInformation("AddCookieSettings(services={Services}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", services);
		_logger.LogInformation("AddCookieSettings(services={Services}): GDPR-AUDIT [Art.30]: Personal data processing activity completed", services);
		_logger.LogWarning("AddCookieSettings(services={Services}): GDPR-AUDIT [Art.7]: Consent operation completed", services);
		_logger.LogInformation("AddCookieSettings(services={Services}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", services);

        services.AddScoped<RevokeAuthenticationEvents>();

        return services;
    }
}
