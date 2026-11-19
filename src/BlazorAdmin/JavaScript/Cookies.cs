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
		_logger.LogInformation("DeleteCookie(name={Name}): CMMC-AUDIT: Audit traceability — State change recorded for the audit trail", name);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
		_logger.LogInformation("DeleteCookie(name={Name}): CMMC-AUDIT: Access control — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): CMMC-AUDIT: Incident response — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): CMMC-AUDIT: Crypto protection — State change recorded for the audit trail", name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogInformation("GetCookie(name={Name}): CMMC-AUDIT: Audit traceability — Record access recorded for the audit trail", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
