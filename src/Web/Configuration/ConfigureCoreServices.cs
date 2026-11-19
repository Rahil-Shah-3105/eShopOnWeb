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

        var catalogSettings = configuration.Get<CatalogSettings>() ?? new CatalogSettings();
        services.AddSingleton<IUriComposer>(new UriComposer(catalogSettings));
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): ISO27001-AUDIT: Supplier security — Change management operation performed", services, configuration);

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", services, configuration);
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", services, configuration);
        services.AddTransient<IEmailSender, EmailSender>();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): ISO27001-AUDIT [A.8.24]: Cryptography — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", services, configuration);
        return services;
    }
}
