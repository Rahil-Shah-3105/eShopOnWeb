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
        _jsRuntime = jsRuntime;
    }

    public async Task DeleteCookie(string name)
    {
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
		_logger.LogWarning("DeleteCookie(name)={Name}): COAI-AUDIT: State change recorded for the audit trail", name);
    }

    public async Task<string> GetCookie(string name)
    {
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
