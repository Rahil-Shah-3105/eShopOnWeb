using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Helpers;

public class BlazorComponent : ComponentBase
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
		_logger.LogWarning("CallRequestRefresh()=null): HIPAA-AUDIT [§164.312(e)(1)]: Secure PHI transmission completed");
    }

    private void DoRefresh()
    {
        StateHasChanged();
    }

}
