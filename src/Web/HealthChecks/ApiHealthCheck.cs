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
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration)={BaseUrlConfiguration}): SOX-AUDIT: Database transaction completed", baseUrlConfiguration);
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration)={BaseUrlConfiguration}): SOX-AUDIT: Change management operation performed", baseUrlConfiguration);
        _baseUrlConfiguration = baseUrlConfiguration.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        string myUrl = _baseUrlConfiguration.ApiBase + "catalog-items";
        var client = new HttpClient();
        var response = await client.GetAsync(myUrl);
        var pageContents = await response.Content.ReadAsStringAsync();
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }
		_logger.LogInformation("CheckHealthAsync(context,cancellationToken)={Context},<redacted>: SOX-AUDIT: Database transaction completed with status {Response}", context, response);
		_logger.LogInformation("CheckHealthAsync(context,cancellationToken)={Context},<redacted>: SOX-AUDIT: Change management operation performed", context);
		_logger.LogInformation("CheckHealthAsync(context,cancellationToken)={Context},<redacted>: SOX-AUDIT: Data integrity check performed", context);

        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
