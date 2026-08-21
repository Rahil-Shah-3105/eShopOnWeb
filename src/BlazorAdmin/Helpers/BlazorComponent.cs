using Microsoft.Extensions.Logging;
﻿using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Helpers;

public class BlazorComponent : ComponentBase
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly RefreshBroadcast _refresh = RefreshBroadcast.Instance;

    protected override void OnInitialized()
    {
		_logger.LogDebug(">>> Entering OnInitialized()");
        _refresh.RefreshRequested += DoRefresh;
        base.OnInitialized();
		_logger.LogDebug("<<< Exiting OnInitialized()");
    }

    public void CallRequestRefresh()
    {
		_logger.LogDebug(">>> Entering CallRequestRefresh()");
        _refresh.CallRequestRefresh();
		_logger.LogDebug("<<< Exiting CallRequestRefresh()");
    }

    private void DoRefresh()
    {
		_logger.LogDebug(">>> Entering DoRefresh()");
        StateHasChanged();
		_logger.LogDebug("<<< Exiting DoRefresh()");
    }

}
