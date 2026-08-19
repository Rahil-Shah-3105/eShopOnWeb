using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.JavaScript;

public class Cookies
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IJSRuntime _jsRuntime;

    public Cookies(IJSRuntime jsRuntime)
    {
		_logger.LogDebug(">>> Entering Cookies(jsRuntime={JsRuntime})", jsRuntime);
        _jsRuntime = jsRuntime;
		_logger.LogDebug("Cookies(jsRuntime={JsRuntime}): _jsRuntime → {_jsRuntime}", jsRuntime, _jsRuntime);
		_logger.LogDebug("<<< Exiting Cookies(jsRuntime={JsRuntime})", jsRuntime);
    }

    public async Task DeleteCookie(string name)
    {
		_logger.LogDebug(">>> Entering DeleteCookie(name={Name})", name);
		_logger.LogInformation("DeleteCookie(name)={Name}: external service call took {Arg1} ms", name, __logStart.ElapsedMilliseconds);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
		_logger.LogDebug("<<< Exiting DeleteCookie(name={Name})", name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogDebug(">>> Entering GetCookie(name={Name})", name);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
		_logger.LogInformation("GetCookie(name)={Name}: external service call took {Arg1} ms", name, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("<<< Exiting GetCookie(name={Name})", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
