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
		_logger.LogDebug(">>> Entering HomePageHealthCheck(httpContextAccessor={HttpContextAccessor})", httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
		_logger.LogDebug("HomePageHealthCheck(httpContextAccessor={HttpContextAccessor}): _httpContextAccessor → {_httpContextAccessor}", httpContextAccessor, _httpContextAccessor);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var request = _httpContextAccessor.HttpContext?.Request;
		_logger.LogDebug(">>> Entering CheckHealthAsync(context={Context},cancellationToken=<redacted>)", context);
        string myUrl = request?.Scheme + "://" + request?.Host.ToString();

        var client = new HttpClient();
		_logger.LogDebug("CheckHealthAsync(context={Context},cancellationToken=<redacted>): client → {Client}", context, client);
        var response = await client.GetAsync(myUrl);
		_logger.LogDebug("CheckHealthAsync(context={Context},cancellationToken=<redacted>): response → {Response}", context, response);
        var pageContents = await response.Content.ReadAsStringAsync();
		_logger.LogDebug("CheckHealthAsync(context={Context},cancellationToken=<redacted>): pageContents → {PageContents}", context, pageContents);
        if (pageContents.Contains(".NET Bot Black Sweatshirt"))
        {
		_logger.LogDebug("<<< Exiting CheckHealthAsync(context={Context},cancellationToken=<redacted>)", context);
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }

		_logger.LogInformation("CheckHealthAsync(context,cancellationToken)={Context},{CancellationToken}: external service call {MyUrl} took {Arg3} ms", context, cancellationToken, myUrl, __logStart.ElapsedMilliseconds);
        return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
    }
}
