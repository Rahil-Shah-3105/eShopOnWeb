using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ConfirmEmailModel : PageModel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
    {
		_logger.LogInformation("ENTERING: ConfirmEmailModel() method");
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
		_logger.LogInformation("ENTERING: OnGetAsync() method");
        if (userId == null || code == null)
        {
			_logger.LogWarning("ENTERING: OnGetAsync() method");
            return RedirectToPage("/Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
        }

		_logger.LogWarning("OnGetAsync(userId,code)={UserId},{Code}: GDPR-AUDIT: Outbound notification recorded for the audit trail {UserId1}", userId, code, userId);
		_logger.LogInformation("OnGetAsync(userId,code)={UserId},{Code}: GDPR-AUDIT [Art.30]: Personal data processing operation recorded {UserId1}", userId, code, userId);
        return Page();
    }
}
