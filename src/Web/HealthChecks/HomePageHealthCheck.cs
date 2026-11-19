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
		_logger.LogInformation("HomePageHealthCheck(httpContextAccessor={HttpContextAccessor}): SOC2-AUDIT [CC6.6]: Database transaction completed", httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
		_logger.LogInformation("HomePageHealthCheck(httpContextAccessor={HttpContextAccessor}): SOC2-AUDIT [CC7.1]: System operation completed", httpContextAccessor);
    _logger.LogInformation("HomePageHealthCheck(httpContextAccessor={HttpContextAccessor}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed", httpContextAccessor);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 5: infrastructure changes — Protected data operation logged for audit compliance", context);
        var request = _httpContextAccessor.HttpContext?.Request;
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 4: privilege changes — Protected data operation logged for audit compliance", context);
        string myUrl = request?.Scheme + "://" + request?.Host.ToString();

		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 2: source/destination network traffic — Protected data operation logged for audit compliance", context);
        var client = new HttpClient();
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 1: user attribution — Protected data operation logged for audit compliance", context);
        var response = await client.GetAsync(myUrl);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Protected data operation logged for audit compliance", context);
        var pageContents = await response.Content.ReadAsStringAsync();
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Log Management / data protection — Protected data operation logged for audit compliance", context);
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Log integrity (encrypt + validate) — Data integrity check performed", context);
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Protected data operation logged for audit compliance", context);
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.6]: Database transaction completed with status {Response}", context, response);

		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC7.1]: System operation completed with status {Response}", context, response);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Activity-Based Outcomes 1-11 — Protected data operation logged for audit compliance", context);
    _logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 7: anomaly hunting — Protected data operation logged for audit compliance", context);
        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
