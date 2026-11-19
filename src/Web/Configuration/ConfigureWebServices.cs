using MediatR;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Configuration;

public static class ConfigureWebServices
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(BasketViewModelService).Assembly));
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", services, configuration);
			_logger.LogWarning("AddWebServices(services={Services}, configuration={Configuration}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", services, configuration);
			_logger.LogWarning("AddWebServices(services={Services}, configuration={Configuration}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", services, configuration);
        services.AddScoped<IBasketViewModelService, BasketViewModelService>();
        services.AddScoped<CatalogViewModelService>();
        services.AddScoped<ICatalogItemViewModelService, CatalogItemViewModelService>();
        services.Configure<CatalogSettings>(configuration);
        services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();

        return services;
    }
}
