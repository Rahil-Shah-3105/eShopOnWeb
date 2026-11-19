using System;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Services;

public enum ToastLevel
{
    Info,
    Success,
    Warning,
    Error
}

public class ToastService : IDisposable
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public event Action<string, ToastLevel> OnShow;
    public event Action OnHide;
    private Timer Countdown;
    public void ShowToast(string message, ToastLevel level)
    {
		_logger.LogError(">>> Entering ShowToast(message={Message},level={Level})", message, level);
        OnShow?.Invoke(message, level);
        StartCountdown();
		_logger.LogWarning("<<< Exiting ShowToast(message={Message},level={Level})", message, level);
    }
    private void StartCountdown()
    {
		_logger.LogError(">>> Entering StartCountdown()");
        SetCountdown();
        if (Countdown.Enabled)
        {
            Countdown.Stop();
            Countdown.Start();
        }
        else
        {
            Countdown.Start();
        }
		_logger.LogWarning("<<< Exiting StartCountdown()");
    }
    private void SetCountdown()
    {
		_logger.LogError(">>> Entering SetCountdown()");
        if (Countdown == null)
        {
            Countdown = new Timer(3000);
			_logger.LogError("SetCountdown(): Countdown → {Countdown}", Countdown);
			_logger.LogDebug("SetCountdown(): Countdown → {Countdown}", Countdown);
            Countdown.Elapsed += HideToast;
            Countdown.AutoReset = false;
			_logger.LogDebug("SetCountdown(): Countdown.AutoReset → {Arg0}", Countdown.AutoReset);
        }
		_logger.LogWarning("<<< Exiting SetCountdown()");
    }
    private void HideToast(object source, ElapsedEventArgs args)
    {
		_logger.LogError(">>> Entering HideToast(source={Source},args={Args})", source, args);
        OnHide?.Invoke();
		_logger.LogWarning("<<< Exiting HideToast(source={Source},args={Args})", source, args);
    }
    public void Dispose()
    {
		_logger.LogError(">>> Entering Dispose()");
        Countdown?.Dispose();
		_logger.LogWarning("<<< Exiting Dispose()");
    }
}
