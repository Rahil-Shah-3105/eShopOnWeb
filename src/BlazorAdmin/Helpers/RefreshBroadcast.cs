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
	_logger.LogInformation("ENTERING: RefreshBroadcast() method");
    }

    public event Action RefreshRequested;
    public void CallRequestRefresh()
    {
		_logger.LogInformation("ENTERING: CallRequestRefresh() method");
        RefreshRequested?.Invoke();
    }
}
