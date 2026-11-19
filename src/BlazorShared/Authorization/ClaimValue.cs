using Microsoft.Extensions.Logging;
namespace BlazorShared.Authorization;

public class ClaimValue
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ClaimValue()
    {
    }

    public ClaimValue(string type, string value)
    {
		_logger.LogError(">>> Entering ClaimValue(type={Type},value={Value})", type, value);
        Type = type;
		_logger.LogWarning("<<< Exiting ClaimValue(type={Type},value={Value})", type, value);
        Value = value;
		_logger.LogDebug("ClaimValue(type={Type},value={Value}): Value → {Value1}", type, value, Value);
    }

    public string Type { get; set; }
    public string Value { get; set; }
}
