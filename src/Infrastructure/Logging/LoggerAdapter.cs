using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Logging;

public class LoggerAdapter<T> : IAppLogger<T>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly ILogger<T> _logger;
    public LoggerAdapter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }

    public void LogWarning(string message, params object[] args)
    {
		_logger.LogInformation("LogWarning(message={Message}, args={Args}): NIS2-AUDIT: Security events — Security event recorded", message, args);
        _logger.LogWarning(message, args);
		_logger.LogInformation("LogWarning(message={Message}, args={Args}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — Security event recorded", message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
		_logger.LogInformation("LogInformation(message={Message}, args={Args}): NIS2-AUDIT: Security events — Security event recorded", message, args);
        _logger.LogInformation(message, args);
		_logger.LogInformation("LogInformation(message={Message}, args={Args}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — Security event recorded", message, args);
    }
}
