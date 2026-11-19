using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Helpers;

public class BlazorComponent : ComponentBase
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly RefreshBroadcast _refresh = RefreshBroadcast.Instance;

    protected override void OnInitialized()
    {
		_logger.LogError(">>> Entering OnInitialized()");
        _refresh.RefreshRequested += DoRefresh;
        base.OnInitialized();
		_logger.LogWarning("<<< Exiting OnInitialized()");
    }

    public void CallRequestRefresh()
    {
		_logger.LogError(">>> Entering CallRequestRefresh()");
        _refresh.CallRequestRefresh();
		_logger.LogWarning("<<< Exiting CallRequestRefresh()");
    }

    private void DoRefresh()
    {
		_logger.LogError(">>> Entering DoRefresh()");
        StateHasChanged();
		_logger.LogWarning("<<< Exiting DoRefresh()");
    }

}
