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
		_logger.LogInformation("ENTERING: Cookies() method");
        _jsRuntime = jsRuntime;
    }

    public async Task DeleteCookie(string name)
    {
		_logger.LogInformation("ENTERING: DeleteCookie() method");
		_logger.LogWarning("DeleteCookie(name)={Name}): GDPR-AUDIT [Art.17]: Data subject right exercised — erasure/portability request processed", name);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogInformation("ENTERING: GetCookie() method");
		_logger.LogInformation("GetCookie(name)={Name}): GDPR-AUDIT [Art.30]: Personal data processing activity completed", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
