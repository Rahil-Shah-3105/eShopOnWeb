using System;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Services;

public enum ToastLevel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    Info,
    Success,
    Warning,
    Error
}

public class ToastService : IDisposable
{
    public event Action<string, ToastLevel> OnShow;
    public event Action OnHide;
    private Timer Countdown;
    public void ShowToast(string message, ToastLevel level)
    {
		_logger.LogDebug(">>> Entering ShowToast(message={Message},level={Level})", message, level);
        OnShow?.Invoke(message, level);
        StartCountdown();
		_logger.LogDebug("<<< Exiting ShowToast(message={Message},level={Level})", message, level);
    }
    private void StartCountdown()
    {
		_logger.LogDebug(">>> Entering StartCountdown()");
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
		_logger.LogDebug("<<< Exiting StartCountdown()");
    }
    private void SetCountdown()
    {
		_logger.LogDebug(">>> Entering SetCountdown()");
        if (Countdown == null)
        {
            Countdown = new Timer(3000);
            Countdown.Elapsed += HideToast;
			_logger.LogDebug("SetCountdown(): Countdown → {Countdown}", Countdown);
			_logger.LogDebug("SetCountdown(): Countdown.AutoReset → {Arg0}", Countdown.AutoReset);
            Countdown.AutoReset = false;
        }
		_logger.LogDebug("<<< Exiting SetCountdown()");
    }
    private void HideToast(object source, ElapsedEventArgs args)
    {
		_logger.LogDebug(">>> Entering HideToast(source={Source},args={Args})", source, args);
        OnHide?.Invoke();
		_logger.LogDebug("<<< Exiting HideToast(source={Source},args={Args})", source, args);
    }
    public void Dispose()
    {
		_logger.LogDebug(">>> Entering Dispose()");
        Countdown?.Dispose();
		_logger.LogDebug("<<< Exiting Dispose()");
    }
}
