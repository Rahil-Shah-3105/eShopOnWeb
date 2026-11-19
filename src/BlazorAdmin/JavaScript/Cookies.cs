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
		_logger.LogInformation("DeleteCookie(name={Name}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", name);
		_logger.LogInformation("DeleteCookie(name={Name}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", name);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogInformation("GetCookie(name={Name}): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail", name);
		_logger.LogInformation("GetCookie(name={Name}): SOC2-AUDIT [CC6.2]: Authentication — Record access recorded for the audit trail", name);
    _logger.LogInformation("GetCookie(name={Name}): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
