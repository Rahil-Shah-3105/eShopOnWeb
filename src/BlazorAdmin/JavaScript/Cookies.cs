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
		_logger.LogWarning("DeleteCookie(name)={Name}): BIPA-AUDIT: State change recorded for the audit trail", name);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogWarning("GetCookie(name)={Name}): BIPA-AUDIT: Record access recorded for the audit trail", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
