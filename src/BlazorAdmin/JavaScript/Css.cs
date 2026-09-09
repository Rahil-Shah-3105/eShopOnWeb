using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.JavaScript;

public class Css
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IJSRuntime _jsRuntime;

    public Css(IJSRuntime jsRuntime)
    {
		_logger.LogInformation("ENTERING: Css() method");
        _jsRuntime = jsRuntime;
    }

    public async Task ShowBodyOverflow()
    {
		_logger.LogInformation("ENTERING: ShowBodyOverflow() method");
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.ShowBodyOverflow);
    }

    public async Task<string> HideBodyOverflow()
    {
		_logger.LogInformation("ENTERING: HideBodyOverflow() method");
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.HideBodyOverflow);
    }
}
