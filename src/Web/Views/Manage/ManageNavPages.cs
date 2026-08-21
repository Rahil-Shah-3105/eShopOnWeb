using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Views.Manage;

public static class ManageNavPages
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static string ActivePageKey => "ActivePage";

    public static string Index => "Index";

    public static string ChangePassword => "ChangePassword";

    public static string ExternalLogins => "ExternalLogins";

    public static string TwoFactorAuthentication => "TwoFactorAuthentication";

    public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

    public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

    public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

    public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

    public static string PageNavClass(ViewContext viewContext, string page)
    {
		_logger.LogDebug(">>> Entering PageNavClass(viewContext={ViewContext},page={Page})", viewContext, page);
        var activePage = viewContext.ViewData["ActivePage"] as string;
		_logger.LogDebug("PageNavClass(viewContext={ViewContext},page={Page}): activePage → {ActivePage}", viewContext, page, activePage);
		_logger.LogDebug("<<< Exiting PageNavClass(viewContext={ViewContext},page={Page})", viewContext, page);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : string.Empty;
    }

    public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
}
