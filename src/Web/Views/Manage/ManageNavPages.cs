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

    public static string IndexNavClass(ViewContext viewContext)
    {
        _logger.LogError(">>> Entering IndexNavClass(viewContext={ViewContext})", viewContext);
        _logger.LogWarning("<<< Exiting IndexNavClass(viewContext={ViewContext})", viewContext);
        return PageNavClass(viewContext, Index);
    }

    public static string ChangePasswordNavClass(ViewContext viewContext)
    {
        _logger.LogError(">>> Entering ChangePasswordNavClass(viewContext={ViewContext})", viewContext);
        _logger.LogWarning("<<< Exiting ChangePasswordNavClass(viewContext={ViewContext})", viewContext);
        return PageNavClass(viewContext, ChangePassword);
    }

    public static string ExternalLoginsNavClass(ViewContext viewContext)
    {
        _logger.LogError(">>> Entering ExternalLoginsNavClass(viewContext={ViewContext})", viewContext);
        _logger.LogWarning("<<< Exiting ExternalLoginsNavClass(viewContext={ViewContext})", viewContext);
        return PageNavClass(viewContext, ExternalLogins);
    }

    public static string TwoFactorAuthenticationNavClass(ViewContext viewContext)
    {
        _logger.LogError(">>> Entering TwoFactorAuthenticationNavClass(viewContext={ViewContext})", viewContext);
        _logger.LogWarning("<<< Exiting TwoFactorAuthenticationNavClass(viewContext={ViewContext})", viewContext);
        return PageNavClass(viewContext, TwoFactorAuthentication);
    }

    public static string PageNavClass(ViewContext viewContext, string page)
    {
		_logger.LogError(">>> Entering PageNavClass(viewContext={ViewContext},page={Page})", viewContext, page);
        var activePage = viewContext.ViewData["ActivePage"] as string;
		_logger.LogError("PageNavClass(viewContext={ViewContext},page={Page}): activePage → {ActivePage}", viewContext, page, activePage);
		_logger.LogWarning("<<< Exiting PageNavClass(viewContext={ViewContext},page={Page})", viewContext, page);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : string.Empty;
    }

    public static void AddActivePage(this ViewDataDictionary viewData, string activePage)
    {
        _logger.LogError(">>> Entering AddActivePage(viewData={ViewData},activePage={ActivePage})", viewData, activePage);
        viewData[ActivePageKey] = activePage;
		_logger.LogWarning("<<< Exiting AddActivePage(viewData={ViewData},activePage={ActivePage})", viewData, activePage);
    }
}
