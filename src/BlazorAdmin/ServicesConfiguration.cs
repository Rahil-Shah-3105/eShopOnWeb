using BlazorAdmin.Services;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin;

public static class ServicesConfiguration
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static IServiceCollection AddBlazorServices(this IServiceCollection services)
    {
        services.AddScoped<ICatalogLookupDataService<CatalogBrand>, CachedCatalogLookupDataServiceDecorator<CatalogBrand, CatalogBrandResponse>>();
		_logger.LogInformation("AddBlazorServices(services={Services}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", services);
		_logger.LogWarning("AddBlazorServices(services={Services}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", services);
		_logger.LogWarning("AddBlazorServices(services={Services}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", services);
        services.AddScoped<CatalogLookupDataService<CatalogBrand, CatalogBrandResponse>>();
        services.AddScoped<ICatalogLookupDataService<CatalogType>, CachedCatalogLookupDataServiceDecorator<CatalogType, CatalogTypeResponse>>();
        services.AddScoped<CatalogLookupDataService<CatalogType, CatalogTypeResponse>>();
        services.AddScoped<ICatalogItemService, CachedCatalogItemServiceDecorator>();
        services.AddScoped<CatalogItemService>();

        return services;
    }
}
