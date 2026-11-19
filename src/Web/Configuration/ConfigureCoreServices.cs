using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Data.Queries;
using Microsoft.eShopWeb.Infrastructure.Logging;
using Microsoft.eShopWeb.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Configuration;

public static class ConfigureCoreServices
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static IServiceCollection AddCoreServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IBasketQueryService, BasketQueryService>();

        var catalogSettings = configuration.Get<CatalogSettings>() ?? new CatalogSettings();
        services.AddSingleton<IUriComposer>(new UriComposer(catalogSettings));

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail", services, configuration);
        services.AddTransient<IEmailSender, EmailSender>();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail", services, configuration);
        return services;
    }
}
