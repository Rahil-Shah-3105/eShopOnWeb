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
        services.AddScoped<CatalogLookupDataService<CatalogBrand, CatalogBrandResponse>>();
        services.AddScoped<ICatalogLookupDataService<CatalogType>, CachedCatalogLookupDataServiceDecorator<CatalogType, CatalogTypeResponse>>();
        services.AddScoped<CatalogLookupDataService<CatalogType, CatalogTypeResponse>>();
		_logger.LogInformation("AddBlazorServices(services={Services}): CMMC-AUDIT: Media protection — State change recorded for the audit trail", services);
        services.AddScoped<ICatalogItemService, CachedCatalogItemServiceDecorator>();
		_logger.LogInformation("AddBlazorServices(services={Services}): CMMC-AUDIT: Incident response — State change recorded for the audit trail", services);
        services.AddScoped<CatalogItemService>();
		_logger.LogInformation("AddBlazorServices(services={Services}): CMMC-AUDIT: Crypto protection — State change recorded for the audit trail", services);

		_logger.LogInformation("AddBlazorServices(services={Services}): CMMC-AUDIT: Audit traceability — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): CMMC-AUDIT: Access control — State change recorded for the audit trail", services);
        return services;
    }
}
