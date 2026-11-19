using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.HealthChecks;

public class HomePageHealthCheck : IHealthCheck
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomePageHealthCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
		_logger.LogWarning("HomePageHealthCheck(httpContextAccessor={HttpContextAccessor}): GDPR-AUDIT: Access, erasure, portability — Database transaction completed", httpContextAccessor);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        string myUrl = request?.Scheme + "://" + request?.Host.ToString();

		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): GDPR-AUDIT: Transfers to third countries — Personal data operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 32]: Security of processing — Personal data operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Personal data operation logged for audit compliance", context);
        var client = new HttpClient();
        var response = await client.GetAsync(myUrl);
        var pageContents = await response.Content.ReadAsStringAsync();
		_logger.LogWarning("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): GDPR-AUDIT: Access, erasure, portability — Database transaction completed with status {Response}", context, response);
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 30]: Records of processing activities — Personal data operation logged for audit compliance", context);
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Personal data operation logged for audit compliance", context);
		_logger.LogWarning("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Personal data operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): GDPR-AUDIT: Breach notification — Personal data operation logged for audit compliance", context);

        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
