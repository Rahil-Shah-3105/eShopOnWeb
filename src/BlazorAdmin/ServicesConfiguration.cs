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
        services.AddScoped<ICatalogItemService, CachedCatalogItemServiceDecorator>();
		_logger.LogInformation("AddBlazorServices(services={Services}): PCI-AUDIT [Req. 10.7]: Security control failure — State change recorded for the audit trail", services);
        services.AddScoped<CatalogItemService>();
		_logger.LogInformation("AddBlazorServices(services={Services}): PCI-AUDIT: Script integrity — State change recorded for the audit trail", services);

		_logger.LogInformation("AddBlazorServices(services={Services}): PCI-AUDIT: Authn controls — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): PCI-AUDIT: Audit coverage — State change recorded for the audit trail", services);
        return services;
    }
}
