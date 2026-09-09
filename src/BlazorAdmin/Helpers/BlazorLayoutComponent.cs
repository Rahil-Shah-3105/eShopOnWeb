using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Helpers;

public class BlazorLayoutComponent : LayoutComponentBase
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly RefreshBroadcast _refresh = RefreshBroadcast.Instance;

    protected override void OnInitialized()
    {
		_logger.LogInformation("ENTERING: OnInitialized() method");
        _refresh.RefreshRequested += DoRefresh;
        base.OnInitialized();
    }

    public void CallRequestRefresh()
    {
		_logger.LogInformation("ENTERING: CallRequestRefresh() method");
        _refresh.CallRequestRefresh();
    }

    private void DoRefresh()
    {
		_logger.LogInformation("ENTERING: DoRefresh() method");
        StateHasChanged();
    }
}
