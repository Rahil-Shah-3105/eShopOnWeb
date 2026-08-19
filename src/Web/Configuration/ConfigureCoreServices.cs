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
		_logger.LogDebug(">>> Entering AddCoreServices(services={Services},configuration={Configuration})", services, configuration);
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IBasketQueryService, BasketQueryService>();

        var catalogSettings = configuration.Get<CatalogSettings>() ?? new CatalogSettings();
		_logger.LogDebug("AddCoreServices(services={Services},configuration={Configuration}): catalogSettings → {CatalogSettings}", services, configuration, catalogSettings);
        services.AddSingleton<IUriComposer>(new UriComposer(catalogSettings));

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddTransient<IEmailSender, EmailSender>();

		_logger.LogDebug("<<< Exiting AddCoreServices(services={Services},configuration={Configuration})", services, configuration);
        return services;
    }
}
