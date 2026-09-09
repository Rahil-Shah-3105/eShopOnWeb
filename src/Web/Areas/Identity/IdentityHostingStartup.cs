using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Microsoft.eShopWeb.Web.Areas.Identity.IdentityHostingStartup))]
namespace Microsoft.eShopWeb.Web.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Configure(IWebHostBuilder builder)
    {
		_logger.LogInformation("Configure(builder)={Builder}): M2614-AUDIT: Change management operation performed", builder);
        builder.ConfigureServices((context, services) =>
        {
        });
		_logger.LogInformation("Configure(builder)={Builder}): SOC2-AUDIT [CC8.1]: Change management operation performed", builder);
    }
}
