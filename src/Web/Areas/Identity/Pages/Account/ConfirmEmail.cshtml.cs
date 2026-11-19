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
		_logger.LogError(">>> Entering ConfirmEmailModel(userManager={UserManager})", userManager);
        _userManager = userManager;
		_logger.LogDebug("ConfirmEmailModel(userManager={UserManager}): _userManager → {_userManager}", userManager, _userManager);
    _logger.LogWarning("<<< Exiting ConfirmEmailModel(userManager={UserManager})", userManager);
    }

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
		_logger.LogError(">>> Entering OnGetAsync(userId={UserId},code={Code})", userId, code);
        if (userId == null || code == null)
        {
            return RedirectToPage("/Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
		_logger.LogError("OnGetAsync(userId={UserId},code={Code}): user → {User}", userId, code, user);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
		_logger.LogError("OnGetAsync(userId={UserId},code={Code}): result → {Result}", userId, code, result);
        if (!result.Succeeded)
        {
			_logger.LogError("Throwing InvalidOperationException from OnGetAsync(userId={UserId},code={Code})", userId, code);
            throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
        }

		_logger.LogWarning("<<< Exiting OnGetAsync(userId={UserId},code={Code})", userId, code);
        return Page();
    }
}
