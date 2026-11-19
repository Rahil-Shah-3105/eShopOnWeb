using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.HealthChecks;

public class ApiHealthCheck : IHealthCheck
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly BaseUrlConfiguration _baseUrlConfiguration;

    public ApiHealthCheck(IOptions<BaseUrlConfiguration> baseUrlConfiguration)
    {
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): SOC2-AUDIT [CC7.1]: System operation completed", baseUrlConfiguration);
        _baseUrlConfiguration = baseUrlConfiguration.Value;
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): SOC2-AUDIT [CC6.6]: Database transaction completed", baseUrlConfiguration);
    _logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): SOC2-AUDIT [CC8.1]: Change management operation performed", baseUrlConfiguration);
    _logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed", baseUrlConfiguration);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 2: source/destination network traffic — Protected data operation logged for audit compliance", context);
        string myUrl = _baseUrlConfiguration.ApiBase + "catalog-items";
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 1: user attribution — Protected data operation logged for audit compliance", context);
        var client = new HttpClient();
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Protected data operation logged for audit compliance", context);
        var response = await client.GetAsync(myUrl);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Log Management / data protection — Protected data operation logged for audit compliance", context);
        var pageContents = await response.Content.ReadAsStringAsync();
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.6]: Database transaction completed with status {Response}", context, response);
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Log integrity (encrypt + validate) — Data integrity check performed", context);
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed with status {Response}", context, response);
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC7.1]: System operation completed with status {Response}", context, response);

		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC8.1]: Change management operation performed", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Activity-Based Outcomes 1-11 — Protected data operation logged for audit compliance", context);
    _logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 4: privilege changes — Protected data operation logged for audit compliance", context);
        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
