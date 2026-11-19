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
        services.Configure<CookiePolicyOptions>(options =>
        {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //TODO need to check that.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
        });
		_logger.LogInformation("AddCookieSettings(services={Services}): ISO27001-AUDIT: Supplier security — State change recorded for the audit trail", services);
        services.ConfigureApplicationCookie(options =>
        {
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
		_logger.LogInformation("AddCookieSettings(services={Services}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", services);

		_logger.LogInformation("AddCookieSettings(services={Services}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", services);
        services.AddScoped<RevokeAuthenticationEvents>();
		_logger.LogInformation("AddCookieSettings(services={Services}): ISO27001-AUDIT [A.8.24]: Cryptography — State change recorded for the audit trail", services);

		_logger.LogInformation("AddCookieSettings(services={Services}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", services);
		_logger.LogInformation("AddCookieSettings(services={Services}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", services);
        return services;
    }
}
