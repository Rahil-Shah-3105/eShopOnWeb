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
        services.Configure<CatalogSettings>(configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GLBA-AUDIT: Security program — State change recorded for the audit trail", services, configuration);
        services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GLBA-AUDIT: Encryption disposal — State change recorded for the audit trail", services, configuration);

		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GLBA-AUDIT [16 CFR 314.4(j)]: Breach notification — State change recorded for the audit trail", services, configuration);
		_logger.LogInformation("AddWebServices(services={Services}, configuration={Configuration}): GLBA-AUDIT [16 CFR 314.4(c)(5)]: Access controls — State change recorded for the audit trail", services, configuration);
        return services;
    }
}
