using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.JavaScript;

public class Route
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IJSRuntime _jsRuntime;

    public Route(IJSRuntime jsRuntime)
    {
		_logger.LogError(">>> Entering Route(jsRuntime={JsRuntime})", jsRuntime);
        _jsRuntime = jsRuntime;
		_logger.LogDebug("Route(jsRuntime={JsRuntime}): _jsRuntime → {_jsRuntime}", jsRuntime, _jsRuntime);
    _logger.LogWarning("<<< Exiting Route(jsRuntime={JsRuntime})", jsRuntime);
    }

    public async Task RouteOutside(string path)
    {
		_logger.LogError(">>> Entering RouteOutside(path={Path})", path);
        var __logStart = System.Diagnostics.Stopwatch.StartNew();
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.RouteOutside, path);
		_logger.LogError("RouteOutside(path)={Path}: external service call took {Arg1} ms", path, __logStart.ElapsedMilliseconds);
    _logger.LogWarning("<<< Exiting RouteOutside(path={Path})", path);
    }
}
