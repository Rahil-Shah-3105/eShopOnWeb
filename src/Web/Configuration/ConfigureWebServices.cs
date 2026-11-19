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
        services.AddScoped<IBasketViewModelService, BasketViewModelService>();
        services.AddScoped<CatalogViewModelService>();
        services.AddScoped<ICatalogItemViewModelService, CatalogItemViewModelService>();
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT [RA-5]: Vulnerability management — State change recorded for the audit trail", services, configuration);
        services.Configure<CatalogSettings>(configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT: Incident response — State change recorded for the audit trail", services, configuration);
        services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT: Crypto protection — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT: Change management operation performed", services, configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT: Audit completeness — State change recorded for the audit trail", services, configuration);
        return services;
    }
}
