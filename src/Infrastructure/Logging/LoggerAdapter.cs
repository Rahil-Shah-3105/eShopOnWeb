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
		_logger.LogWarning("LoggerAdapter(loggerFactory)={LoggerFactory}): DORA-AUDIT: Security incident detected", loggerFactory);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
		_logger.LogWarning("LogWarning(message,args)={Message},{Args}: DORA-AUDIT: Security incident detected", message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
		_logger.LogWarning("LogInformation(message,args)={Message},{Args}: DORA-AUDIT: Security incident detected", message, args);
    }
}
