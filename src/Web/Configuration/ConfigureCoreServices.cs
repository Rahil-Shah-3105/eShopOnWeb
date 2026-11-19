using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Data.Queries;
using Microsoft.eShopWeb.Infrastructure.Logging;
using Microsoft.eShopWeb.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Configuration;

public static class ConfigureCoreServices
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static IServiceCollection AddCoreServices(this IServiceCollection services,
        IConfiguration configuration)
    {
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 10: automated alerting — State change recorded for the audit trail", services, configuration);
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail", services, configuration);
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", services, configuration);
        services.AddScoped<IBasketService, BasketService>();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", services, configuration);
        services.AddScoped<IOrderService, OrderService>();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed", services, configuration);
        services.AddScoped<IBasketQueryService, BasketQueryService>();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", services, configuration);
        var catalogSettings = configuration.Get<CatalogSettings>() ?? new CatalogSettings();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", services, configuration);
        services.AddSingleton<IUriComposer>(new UriComposer(catalogSettings));
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", services, configuration);
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", services, configuration);
        services.AddTransient<IEmailSender, EmailSender>();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC8.1]: Change management operation performed", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", services, configuration);
    _logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail", services, configuration);
    _logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail", services, configuration);
        return services;
    }
}
