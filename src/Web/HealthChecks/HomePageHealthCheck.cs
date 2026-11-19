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
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        string myUrl = request?.Scheme + "://" + request?.Host.ToString();

        var client = new HttpClient();
        var response = await client.GetAsync(myUrl);
        var pageContents = await response.Content.ReadAsStringAsync();
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): DORA-AUDIT: Third party risk — Protected data operation logged for audit compliance", context);
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): DORA-AUDIT: Resilience testing — Protected data operation logged for audit compliance", context);

		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): DORA-AUDIT [Art. 45]: Information sharing — Protected data operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): DORA-AUDIT: Incident coverage — Protected data operation logged for audit compliance", context);
        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
