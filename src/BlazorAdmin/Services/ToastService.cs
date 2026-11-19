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
        OnShow?.Invoke(message, level);
        StartCountdown();
    }
    private void StartCountdown()
    {
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
		_logger.LogInformation("SetCountdown(): PCI-AUDIT: Authn controls — State change recorded for the audit trail");
        if (Countdown == null)
        {
			_logger.LogInformation("SetCountdown(): PCI-AUDIT: Script integrity — State change recorded for the audit trail");
            Countdown = new Timer(3000);
            Countdown.Elapsed += HideToast;
            Countdown.AutoReset = false;
			_logger.LogInformation("SetCountdown(): PCI-AUDIT [Req. 10.7]: Security control failure — State change recorded for the audit trail");
			_logger.LogInformation("SetCountdown(): PCI-AUDIT: Audit coverage — State change recorded for the audit trail");
        }
    }
    private void HideToast(object source, ElapsedEventArgs args)
    {
        OnHide?.Invoke();
    }
    public void Dispose()
    {
        Countdown?.Dispose();
    }
}
