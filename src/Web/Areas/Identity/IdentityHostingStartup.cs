using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Microsoft.eShopWeb.Web.Areas.Identity.IdentityHostingStartup))]
namespace Microsoft.eShopWeb.Web.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
        });
		_logger.LogInformation("Configure(builder={Builder}): ISO42001-AUDIT: Lifecycle — Change management operation performed", builder);
    }
}
