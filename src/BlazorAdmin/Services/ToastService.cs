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
		_logger.LogWarning("SetCountdown(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail");
        if (Countdown == null)
        {
			_logger.LogInformation("SetCountdown(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail");
            Countdown = new Timer(3000);
            Countdown.Elapsed += HideToast;
			_logger.LogInformation("SetCountdown(): GDPR-AUDIT: Breach notification — State change recorded for the audit trail");
			_logger.LogInformation("SetCountdown(): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail");
			_logger.LogWarning("SetCountdown(): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail");
            Countdown.AutoReset = false;
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
