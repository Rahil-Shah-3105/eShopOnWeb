using Microsoft.Extensions.Logging;
namespace Microsoft.AspNetCore.Mvc;

public static class UrlHelperExtensions
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static string? EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
    {
		_logger.LogError(">>> Entering EmailConfirmationLink(urlHelper={UrlHelper},userId={UserId},code={Code},scheme={Scheme})", urlHelper, userId, code, scheme);
        _logger.LogWarning("<<< Exiting EmailConfirmationLink(urlHelper={UrlHelper},userId={UserId},code={Code},scheme={Scheme})", urlHelper, userId, code, scheme);
        return urlHelper.Action(
            action: "GET",
            controller: "ConfirmEmail",
            values: new { userId, code },
            protocol: scheme);
    }
}
