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
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOX-AUDIT: Change management (PCAOB AS 2201) — Change management operation performed", services, configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", services, configuration);
        services.AddScoped<ICatalogItemViewModelService, CatalogItemViewModelService>();
        services.Configure<CatalogSettings>(configuration);
        services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();

        return services;
    }
}
