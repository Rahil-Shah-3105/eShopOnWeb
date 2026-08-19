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
		_logger.LogDebug(">>> Entering Css(jsRuntime={JsRuntime})", jsRuntime);
        _jsRuntime = jsRuntime;
		_logger.LogDebug("Css(jsRuntime={JsRuntime}): _jsRuntime → {_jsRuntime}", jsRuntime, _jsRuntime);
		_logger.LogDebug("<<< Exiting Css(jsRuntime={JsRuntime})", jsRuntime);
    }

    public async Task ShowBodyOverflow()
    {
		_logger.LogDebug(">>> Entering ShowBodyOverflow()");
		_logger.LogInformation("ShowBodyOverflow(): external service call took {Arg0} ms", __logStart.ElapsedMilliseconds);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.ShowBodyOverflow);
		_logger.LogDebug("<<< Exiting ShowBodyOverflow()");
    }

    public async Task<string> HideBodyOverflow()
    {
		_logger.LogDebug(">>> Entering HideBodyOverflow()");
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
		_logger.LogInformation("HideBodyOverflow(): external service call took {Arg0} ms", __logStart.ElapsedMilliseconds);
		_logger.LogDebug("<<< Exiting HideBodyOverflow()");
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.HideBodyOverflow);
    }
}
