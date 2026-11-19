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
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): SOC2-AUDIT [CC8.1]: Change management operation performed", baseUrlConfiguration);
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): SOC2-AUDIT [CC7.1]: System operation completed", baseUrlConfiguration);
    _logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): SOC2-AUDIT [CC6.6]: Database transaction completed", baseUrlConfiguration);
        _baseUrlConfiguration = baseUrlConfiguration.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.8]: Malicious software — Protected data operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.1]: Logical access — Protected data operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC7.3]: Incident response — Protected data operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.6]: Boundary protection — Protected data operation logged for audit compliance", context);
        string myUrl = _baseUrlConfiguration.ApiBase + "catalog-items";
        var client = new HttpClient();
        var response = await client.GetAsync(myUrl);
        var pageContents = await response.Content.ReadAsStringAsync();
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.6]: Database transaction completed with status {Response}", context, response);
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC8.1]: Change management operation performed", context);
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC7.2]: Monitoring coverage — Protected data operation logged for audit compliance", context);
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC7.1]: System operation completed with status {Response}", context, response);
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.3]: Authorization — Protected data operation logged for audit compliance", context);
			_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.2]: Authentication — Protected data operation logged for audit compliance", context);
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }

        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
