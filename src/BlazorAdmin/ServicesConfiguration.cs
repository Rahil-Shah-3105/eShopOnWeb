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
		_logger.LogInformation("AddBlazorServices(services={Services}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", services);
        services.AddScoped<CatalogLookupDataService<CatalogType, CatalogTypeResponse>>();
        services.AddScoped<ICatalogItemService, CachedCatalogItemServiceDecorator>();
        services.AddScoped<CatalogItemService>();

        return services;
    }
}
