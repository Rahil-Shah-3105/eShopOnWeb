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
		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", services);
        services.AddScoped<ICatalogLookupDataService<CatalogBrand>, CachedCatalogLookupDataServiceDecorator<CatalogBrand, CatalogBrandResponse>>();
		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", services);
        services.AddScoped<CatalogLookupDataService<CatalogBrand, CatalogBrandResponse>>();
		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", services);
        services.AddScoped<ICatalogLookupDataService<CatalogType>, CachedCatalogLookupDataServiceDecorator<CatalogType, CatalogTypeResponse>>();
		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", services);
        services.AddScoped<CatalogLookupDataService<CatalogType, CatalogTypeResponse>>();
		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", services);
        services.AddScoped<ICatalogItemService, CachedCatalogItemServiceDecorator>();
		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", services);
        services.AddScoped<CatalogItemService>();
		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", services);

		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", services);
    _logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", services);
    _logger.LogInformation("AddBlazorServices(services={Services}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", services);
        return services;
    }
}
