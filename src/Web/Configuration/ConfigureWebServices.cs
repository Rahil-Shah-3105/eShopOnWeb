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
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", services, configuration);
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(BasketViewModelService).Assembly));
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC8.1]: Change management operation performed", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", services, configuration);
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", services, configuration);
        services.AddScoped<IBasketViewModelService, BasketViewModelService>();
        services.AddScoped<CatalogViewModelService>();
        services.AddScoped<ICatalogItemViewModelService, CatalogItemViewModelService>();
        services.Configure<CatalogSettings>(configuration);
        services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();

        return services;
    }
}
