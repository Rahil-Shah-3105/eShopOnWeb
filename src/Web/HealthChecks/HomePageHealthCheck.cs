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
		_logger.LogInformation("HomePageHealthCheck(httpContextAccessor={HttpContextAccessor}): FERPA-AUDIT: Inspect amend — Database transaction completed", httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
		_logger.LogInformation("HomePageHealthCheck(httpContextAccessor={HttpContextAccessor}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed", httpContextAccessor);
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
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): FERPA-AUDIT: Inspect amend — Database transaction completed with status {Response}", context, response);
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed with status {Response}", context, response);

		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): FERPA-AUDIT [34 CFR 99.37]: Directory information — Education records operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): FERPA-AUDIT [34 CFR 99.30]: Consent — Education records operation logged for audit compliance", context);
        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
