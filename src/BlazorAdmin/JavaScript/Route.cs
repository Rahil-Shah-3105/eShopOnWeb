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
		_logger.LogInformation("ENTERING: Route() method");
        _jsRuntime = jsRuntime;
    }

    public async Task RouteOutside(string path)
    {
		_logger.LogInformation("ENTERING: RouteOutside() method");
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.RouteOutside, path);
    }
}
