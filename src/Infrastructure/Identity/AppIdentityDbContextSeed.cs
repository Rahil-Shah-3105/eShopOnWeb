using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static async Task SeedAsync(AppIdentityDbContext identityDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
	_logger.LogDebug(">>> Entering SeedAsync(identityDbContext={IdentityDbContext},userManager={UserManager},roleManager={RoleManager})", identityDbContext, userManager, roleManager);

        if (identityDbContext.Database.IsSqlServer())
        {
            identityDbContext.Database.Migrate();
        }

        await roleManager.CreateAsync(new IdentityRole(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS));

        var defaultUser = new ApplicationUser { UserName = "demouser@microsoft.com", Email = "demouser@microsoft.com" };
		_logger.LogDebug("SeedAsync(identityDbContext={IdentityDbContext},userManager={UserManager},roleManager={RoleManager}): defaultUser → {DefaultUser}", identityDbContext, userManager, roleManager, defaultUser);
        await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);

        string adminUserName = "admin@microsoft.com";
        var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
		_logger.LogDebug("SeedAsync(identityDbContext={IdentityDbContext},userManager={UserManager},roleManager={RoleManager}): adminUser → {AdminUser}", identityDbContext, userManager, roleManager, adminUser);
        await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
        adminUser = await userManager.FindByNameAsync(adminUserName);
        if (adminUser != null)
        {
            await userManager.AddToRoleAsync(adminUser, BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
        }
		_logger.LogDebug("<<< Exiting SeedAsync(identityDbContext={IdentityDbContext},userManager={UserManager},roleManager={RoleManager})", identityDbContext, userManager, roleManager);
    }
}
