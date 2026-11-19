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
		_logger.LogInformation("AddBlazorServices(services={Services}): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail", services);
        services.AddScoped<CatalogItemService>();
		_logger.LogInformation("AddBlazorServices(services={Services}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail", services);

		_logger.LogInformation("AddBlazorServices(services={Services}): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail", services);
		_logger.LogInformation("AddBlazorServices(services={Services}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail", services);
        return services;
    }
}
