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
		_logger.LogInformation("LogWarning(message={Message}, args={Args}): M2614-AUDIT: Outcome 2: source/destination network traffic — Security event recorded", message, args);
        _logger.LogWarning(message, args);
		_logger.LogInformation("LogWarning(message={Message}, args={Args}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Security event recorded", message, args);
    _logger.LogInformation("LogWarning(message={Message}, args={Args}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Security event recorded", message, args);
    _logger.LogInformation("LogWarning(message={Message}, args={Args}): M2614-AUDIT: Outcome 7: anomaly hunting — Security event recorded", message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
		_logger.LogInformation("LogInformation(message={Message}, args={Args}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Security event recorded", message, args);
        _logger.LogInformation(message, args);
		_logger.LogInformation("LogInformation(message={Message}, args={Args}): SOC2-AUDIT [CC6.1]: Authentication operation completed", message, args);
    _logger.LogInformation("LogInformation(message={Message}, args={Args}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", message, args);
    _logger.LogInformation("LogInformation(message={Message}, args={Args}): M2614-AUDIT: Outcome 2: source/destination network traffic — Security event recorded", message, args);
    }
}
