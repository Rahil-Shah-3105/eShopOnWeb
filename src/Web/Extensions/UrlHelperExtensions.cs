using Microsoft.Extensions.Logging;
namespace Microsoft.AspNetCore.Mvc;

public static class UrlHelperExtensions
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static string? EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
    {
		_logger.LogInformation("ENTERING: EmailConfirmationLink() method");
        return urlHelper.Action(
            action: "GET",
            controller: "ConfirmEmail",
            values: new { userId, code },
            protocol: scheme);
    }
}
