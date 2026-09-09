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
		_logger.LogInformation("ENTERING: ShowToast() method");
        OnShow?.Invoke(message, level);
        StartCountdown();
    }
    private void StartCountdown()
    {
		_logger.LogInformation("ENTERING: StartCountdown() method");
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
    }
    private void SetCountdown()
    {
		_logger.LogInformation("ENTERING: SetCountdown() method");
        if (Countdown == null)
        {
            Countdown = new Timer(3000);
            Countdown.Elapsed += HideToast;
            Countdown.AutoReset = false;
        }
		_logger.LogWarning("SetCountdown()=null): GDPR-AUDIT: State change recorded for the audit trail");
    }
    private void HideToast(object source, ElapsedEventArgs args)
    {
		_logger.LogInformation("ENTERING: HideToast() method");
        OnHide?.Invoke();
    }
    public void Dispose()
    {
		_logger.LogInformation("ENTERING: Dispose() method");
        Countdown?.Dispose();
    }
}
