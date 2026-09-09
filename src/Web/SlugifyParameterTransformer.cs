using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string? TransformOutbound(object? value)
    {
		_logger.LogInformation("ENTERING: TransformOutbound() method");
        if (value == null) { return null; }
        string? str = value.ToString();
        if (string.IsNullOrEmpty(str)) { return null; }

        // Slugify value
        return Regex.Replace(str, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
