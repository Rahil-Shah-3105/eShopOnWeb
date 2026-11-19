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

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT [RA-5]: Vulnerability management — State change recorded for the audit trail", services, configuration);
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT: Incident response — State change recorded for the audit trail", services, configuration);
        services.AddTransient<IEmailSender, EmailSender>();
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT: Crypto protection — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT: Change management operation performed", services, configuration);
		_logger.LogInformation("AddCoreServices(services={Services}, configuration={Configuration}): FEDRAMP-AUDIT: Audit completeness — State change recorded for the audit trail", services, configuration);
        return services;
    }
}
