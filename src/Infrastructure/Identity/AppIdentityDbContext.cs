using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Microsoft.eShopWeb.Infrastructure.Identity;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
	_logger.LogDebug(">>> Entering AppIdentityDbContext(options={Options})", options);
	_logger.LogDebug("<<< Exiting AppIdentityDbContext(options={Options})", options);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
		_logger.LogDebug(">>> Entering OnModelCreating(builder={Builder})", builder);
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
		_logger.LogDebug("<<< Exiting OnModelCreating(builder={Builder})", builder);
    }
}
