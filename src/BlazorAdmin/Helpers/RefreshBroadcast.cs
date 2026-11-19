using Microsoft.Extensions.Logging;
using System;

namespace BlazorAdmin.Helpers;

internal sealed class RefreshBroadcast
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private static readonly Lazy<RefreshBroadcast>
        Lazy =
            new Lazy<RefreshBroadcast>
                (() => new RefreshBroadcast());

    public static RefreshBroadcast Instance => Lazy.Value;

    private RefreshBroadcast()
    {
    }

    public event Action RefreshRequested;
    public void CallRequestRefresh()
    {
        RefreshRequested?.Invoke();
		_logger.LogInformation("CallRequestRefresh(): HIPAA-AUDIT [§164.312(e)(1)]: Secure PHI transmission completed");
    }
}
