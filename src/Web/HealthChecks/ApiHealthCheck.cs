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
        _baseUrlConfiguration = baseUrlConfiguration.Value;
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): USSTATE-AUDIT: Consumer rights (VCDPA 59.1-577 et al.) — Database transaction completed", baseUrlConfiguration);
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
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): USSTATE-AUDIT: Opt-in consent for sensitive data — Personal information operation logged for audit compliance", context);

		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): USSTATE-AUDIT: Data protection assessments — Personal information operation logged for audit compliance", context);
		_logger.LogInformation("CheckHealthAsync(context={Context}, cancellationToken=<redacted>): USSTATE-AUDIT: Consumer rights (VCDPA 59.1-577 et al.) — Database transaction completed with status {Response}", context, response);
        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
