using Microsoft.Extensions.Logging;
namespace BlazorShared.Authorization;

public class ClaimValue
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ClaimValue()
    {
	_logger.LogInformation("ENTERING: ClaimValue() method");
    }

    public ClaimValue(string type, string value)
    {
        Type = type;
        Value = value;
    }

    public string Type { get; set; }
    public string Value { get; set; }
}
