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
    }

    public async Task ShowBodyOverflow()
    {
		_logger.LogDebug(">>> Entering ShowBodyOverflow()");
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.ShowBodyOverflow);
    }

    public async Task<string> HideBodyOverflow()
    {
		_logger.LogDebug(">>> Entering HideBodyOverflow()");
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.HideBodyOverflow);
    }
}
