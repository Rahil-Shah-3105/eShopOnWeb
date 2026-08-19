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
		_logger.LogDebug(">>> Entering ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration})", baseUrlConfiguration);
        _baseUrlConfiguration = baseUrlConfiguration.Value;
		_logger.LogDebug("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): _baseUrlConfiguration → {_baseUrlConfiguration}", baseUrlConfiguration, _baseUrlConfiguration);
		_logger.LogDebug("<<< Exiting ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration})", baseUrlConfiguration);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
		_logger.LogDebug(">>> Entering CheckHealthAsync(context={Context},cancellationToken=<redacted>)", context);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        string myUrl = _baseUrlConfiguration.ApiBase + "catalog-items";
        var client = new HttpClient();
		_logger.LogDebug("CheckHealthAsync(context={Context},cancellationToken=<redacted>): client → {Client}", context, client);
        var response = await client.GetAsync(myUrl);
		_logger.LogDebug("CheckHealthAsync(context={Context},cancellationToken=<redacted>): response → {Response}", context, response);
        var pageContents = await response.Content.ReadAsStringAsync();
		_logger.LogDebug("CheckHealthAsync(context={Context},cancellationToken=<redacted>): pageContents → {PageContents}", context, pageContents);
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }

		_logger.LogDebug("<<< Exiting CheckHealthAsync(context={Context},cancellationToken=<redacted>)", context);
		_logger.LogInformation("CheckHealthAsync(context,cancellationToken)={Context},{CancellationToken}: external service call {MyUrl} took {Arg3} ms", context, cancellationToken, myUrl, __logStart.ElapsedMilliseconds);
        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
