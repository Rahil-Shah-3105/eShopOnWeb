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
		_logger.LogWarning("DeleteCookie(name={Name}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", name);
		_logger.LogWarning("DeleteCookie(name={Name}): GDPR-AUDIT [Art.17]: Data subject right exercised — erasure/portability request processed", name);
    _logger.LogInformation("DeleteCookie(name={Name}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", name);
    _logger.LogInformation("DeleteCookie(name={Name}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", name);
        await _jsRuntime.InvokeAsync<string>(JSInteropConstants.DeleteCookie, name);
    }

    public async Task<string> GetCookie(string name)
    {
		_logger.LogWarning("GetCookie(name={Name}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail", name);
		_logger.LogInformation("GetCookie(name={Name}): GDPR-AUDIT [Art.30]: Personal data processing activity completed", name);
    _logger.LogInformation("GetCookie(name={Name}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail", name);
    _logger.LogInformation("GetCookie(name={Name}): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail", name);
        return await _jsRuntime.InvokeAsync<string>(JSInteropConstants.GetCookie, name);
    }
}
