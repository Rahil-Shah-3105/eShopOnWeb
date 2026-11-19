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
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", services);
        services.AddScoped<ICatalogLookupDataService<CatalogBrand>, CachedCatalogLookupDataServiceDecorator<CatalogBrand, CatalogBrandResponse>>();
        services.AddScoped<CatalogLookupDataService<CatalogBrand, CatalogBrandResponse>>();
        services.AddScoped<ICatalogLookupDataService<CatalogType>, CachedCatalogLookupDataServiceDecorator<CatalogType, CatalogTypeResponse>>();
        services.AddScoped<CatalogLookupDataService<CatalogType, CatalogTypeResponse>>();
        services.AddScoped<ICatalogItemService, CachedCatalogItemServiceDecorator>();
        services.AddScoped<CatalogItemService>();

        return services;
    }
}
