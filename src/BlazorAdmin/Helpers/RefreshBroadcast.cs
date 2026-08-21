using Microsoft.Extensions.Logging;
﻿using System;

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
	_logger.LogDebug(">>> Entering RefreshBroadcast()");
    }

    public event Action RefreshRequested;
    public void CallRequestRefresh()
    {
		_logger.LogDebug(">>> Entering CallRequestRefresh()");
        RefreshRequested?.Invoke();
		_logger.LogDebug("<<< Exiting CallRequestRefresh()");
    }
}
