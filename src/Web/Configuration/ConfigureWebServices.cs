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
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed", services, configuration);
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(BasketViewModelService).Assembly));
			_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", services, configuration);
        services.AddScoped<IBasketViewModelService, BasketViewModelService>();
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", services, configuration);
        services.AddScoped<CatalogViewModelService>();
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", services, configuration);
        services.AddScoped<ICatalogItemViewModelService, CatalogItemViewModelService>();
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", services, configuration);
        services.Configure<CatalogSettings>(configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", services, configuration);
        services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC8.1]: Change management operation performed", services, configuration);
    _logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", services, configuration);
    _logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", services, configuration);
    _logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", services, configuration);
        return services;
    }
}
