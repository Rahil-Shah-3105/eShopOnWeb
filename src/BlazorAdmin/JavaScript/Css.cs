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
		_logger.LogError(">>> Entering Css(jsRuntime={JsRuntime})", jsRuntime);
        _jsRuntime = jsRuntime;
		_logger.LogDebug("Css(jsRuntime={JsRuntime}): _jsRuntime → {_jsRuntime}", jsRuntime, _jsRuntime);
    _logger.LogWarning("<<< Exiting Css(jsRuntime={JsRuntime})", jsRuntime);
    }

    public async Task ShowBodyOverflow()
    {
		_logger.LogError(">>> Entering ShowBodyOverflow()");
        var __logStart = System.Diagnostics.Stopwatch.StartNew();
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.ShowBodyOverflow);
		_logger.LogError("ShowBodyOverflow(): external service call took {Arg0} ms", __logStart.ElapsedMilliseconds);
    _logger.LogWarning("<<< Exiting ShowBodyOverflow()");
    }

    public async Task<string> HideBodyOverflow()
    {
		_logger.LogError(">>> Entering HideBodyOverflow()");
        var __logStart = System.Diagnostics.Stopwatch.StartNew();
		_logger.LogError("HideBodyOverflow(): external service call took {Arg0} ms", __logStart.ElapsedMilliseconds);
    _logger.LogWarning("<<< Exiting HideBodyOverflow()");
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.HideBodyOverflow);
    }
}
