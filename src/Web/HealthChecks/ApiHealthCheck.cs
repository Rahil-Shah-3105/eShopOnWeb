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
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): FERPA-AUDIT: Inspect amend — Database transaction completed", baseUrlConfiguration);
        _baseUrlConfiguration = baseUrlConfiguration.Value;
		_logger.LogInformation("ApiHealthCheck(baseUrlConfiguration={BaseUrlConfiguration}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed", baseUrlConfiguration);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        string myUrl = _baseUrlConfiguration.ApiBase + "catalog-items";
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
