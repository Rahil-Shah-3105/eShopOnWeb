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
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", services, configuration);
		_logger.LogWarning("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art.30]: Personal data processing activity completed", services, configuration);
		_logger.LogWarning("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", services, configuration);

        var catalogSettings = configuration.Get<CatalogSettings>() ?? new CatalogSettings();
        services.AddSingleton<IUriComposer>(new UriComposer(catalogSettings));

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}
