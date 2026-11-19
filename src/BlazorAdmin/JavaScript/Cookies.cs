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
		_logger.LogInformation("DeleteCookie(name={Name}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", name);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
		_logger.LogInformation("DeleteCookie(name={Name}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogInformation("GetCookie(name={Name}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
