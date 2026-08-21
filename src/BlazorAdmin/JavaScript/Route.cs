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
		_logger.LogDebug(">>> Entering Route(jsRuntime={JsRuntime})", jsRuntime);
        _jsRuntime = jsRuntime;
		_logger.LogDebug("Route(jsRuntime={JsRuntime}): _jsRuntime → {_jsRuntime}", jsRuntime, _jsRuntime);
    }

    public async Task RouteOutside(string path)
    {
		_logger.LogDebug(">>> Entering RouteOutside(path={Path})", path);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.RouteOutside, path);
    }
}
