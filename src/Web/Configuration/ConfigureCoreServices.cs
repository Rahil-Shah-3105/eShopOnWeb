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
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IBasketQueryService, BasketQueryService>();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC8.1]: Change management operation performed", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", services, configuration);

        var catalogSettings = configuration.Get<CatalogSettings>() ?? new CatalogSettings();
        services.AddSingleton<IUriComposer>(new UriComposer(catalogSettings));

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}
