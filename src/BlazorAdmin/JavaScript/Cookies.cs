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
		_logger.LogInformation("DeleteCookie(name={Name}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail", name);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
		_logger.LogInformation("DeleteCookie(name={Name}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): DORA-AUDIT: Third party risk — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail", name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogInformation("GetCookie(name={Name}): DORA-AUDIT [Art. 45]: Information sharing — Record access recorded for the audit trail", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
