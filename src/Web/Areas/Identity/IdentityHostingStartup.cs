using Microsoft.Extensions.Logging;
﻿using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Microsoft.eShopWeb.Web.Areas.Identity.IdentityHostingStartup))]
namespace Microsoft.eShopWeb.Web.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Configure(IWebHostBuilder builder)
    {
		_logger.LogDebug(">>> Entering Configure(builder={Builder})", builder);
        builder.ConfigureServices((context, services) =>
        {
        });
		_logger.LogDebug("<<< Exiting Configure(builder={Builder})", builder);
    }
}
