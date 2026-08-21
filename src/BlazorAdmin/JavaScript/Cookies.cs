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
    }

    public async Task DeleteCookie(string name)
    {
		_logger.LogDebug(">>> Entering DeleteCookie(name={Name})", name);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogDebug(">>> Entering GetCookie(name={Name})", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
