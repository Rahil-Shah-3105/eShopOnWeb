using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BlazorShared.Models;

public class ErrorDetails
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public override string ToString()
    {
		_logger.LogError(">>> Entering ToString()");
        _logger.LogWarning("<<< Exiting ToString()");
        return JsonSerializer.Serialize(this);
    }
}
