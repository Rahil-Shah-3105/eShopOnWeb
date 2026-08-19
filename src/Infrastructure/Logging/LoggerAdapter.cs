using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Logging;

public class LoggerAdapter<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;
    public LoggerAdapter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
		_logger.LogDebug(">>> Entering LoggerAdapter(loggerFactory={LoggerFactory})", loggerFactory);
		_logger.LogDebug("LoggerAdapter(loggerFactory={LoggerFactory}): _logger → {_logger}", loggerFactory, _logger);
		_logger.LogDebug("<<< Exiting LoggerAdapter(loggerFactory={LoggerFactory})", loggerFactory);
    }

    public void LogWarning(string message, params object[] args)
    {
		_logger.LogDebug(">>> Entering LogWarning(message={Message},args={Args})", message, args);
        _logger.LogWarning(message, args);
		_logger.LogDebug("<<< Exiting LogWarning(message={Message},args={Args})", message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
		_logger.LogDebug(">>> Entering LogInformation(message={Message},args={Args})", message, args);
        _logger.LogInformation(message, args);
		_logger.LogDebug("<<< Exiting LogInformation(message={Message},args={Args})", message, args);
    }
}
