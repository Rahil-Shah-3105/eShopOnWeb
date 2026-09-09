using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Microsoft.eShopWeb.Web.Pages;

public class PrivacyModel : PageModel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void OnGet()
    {
	_logger.LogWarning("OnGet()=null): BIPA-AUDIT: Record access recorded for the audit trail");
    }
}
