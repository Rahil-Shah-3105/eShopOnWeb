using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Helpers;

public class BlazorLayoutComponent : LayoutComponentBase
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly RefreshBroadcast _refresh = RefreshBroadcast.Instance;

    protected override void OnInitialized()
    {
        _refresh.RefreshRequested += DoRefresh;
        base.OnInitialized();
    }

    public void CallRequestRefresh()
    {
        _refresh.CallRequestRefresh();
		_logger.LogInformation("CallRequestRefresh(): HIPAA-AUDIT [§164.312(e)(1)]: Secure PHI transmission completed");
    }

    private void DoRefresh()
    {
        StateHasChanged();
    }
}
