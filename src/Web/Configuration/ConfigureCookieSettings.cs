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
		_logger.LogInformation("AddCookieSettings(services)={Services}): M2614-AUDIT: Database transaction completed", services);
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
		_logger.LogInformation("AddCookieSettings(services)={Services}): M2614-AUDIT: Authentication operation completed", services);

		_logger.LogWarning("AddCookieSettings(services)={Services}): M2614-AUDIT: State change recorded for the audit trail", services);
        services.AddScoped<RevokeAuthenticationEvents>();
		_logger.LogInformation("AddCookieSettings(services)={Services}): SOC2-AUDIT [CC6.6]: Database transaction completed", services);

		_logger.LogInformation("AddCookieSettings(services)={Services}): SOC2-AUDIT [CC6.1]: Authentication operation completed", services);
        return services;
    }
}
