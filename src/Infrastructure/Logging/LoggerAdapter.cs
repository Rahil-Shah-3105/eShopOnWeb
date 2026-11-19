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
		_logger.LogError(">>> Entering LoggerAdapter(loggerFactory={LoggerFactory})", loggerFactory);
    _logger.LogWarning("<<< Exiting LoggerAdapter(loggerFactory={LoggerFactory})", loggerFactory);
    }

    public void LogWarning(string message, params object[] args)
    {
		_logger.LogError(">>> Entering LogWarning(message={Message},args={Args})", message, args);
        _logger.LogWarning(message, args);
		_logger.LogWarning("<<< Exiting LogWarning(message={Message},args={Args})", message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
		_logger.LogError(">>> Entering LogInformation(message={Message},args={Args})", message, args);
        _logger.LogInformation(message, args);
		_logger.LogWarning("<<< Exiting LogInformation(message={Message},args={Args})", message, args);
    }
}
