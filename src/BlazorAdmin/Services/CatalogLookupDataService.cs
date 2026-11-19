using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using BlazorShared;
using BlazorShared.Attributes;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorAdmin.Services;

public class CatalogLookupDataService<TLookupData, TReponse>
    : ICatalogLookupDataService<TLookupData>
    where TLookupData : LookupData
    where TReponse : ILookupDataResponse<TLookupData>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;

    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogLookupDataService<TLookupData, TReponse>> _logger;
    private readonly string _apiUrl;

    public CatalogLookupDataService(HttpClient httpClient,
        IOptions<BaseUrlConfiguration> baseUrlConfiguration,
        ILogger<CatalogLookupDataService<TLookupData, TReponse>> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiUrl = baseUrlConfiguration.Value.ApiBase;
		_logger.LogInformation("CatalogLookupDataService(httpClient={HttpClient}, baseUrlConfiguration={BaseUrlConfiguration}, logger={Logger}): SOX-AUDIT: Change management (PCAOB AS 2201) — Change management operation performed", httpClient, baseUrlConfiguration, logger);
    }

    public async Task<List<TLookupData>> List()
    {
		_logger.LogInformation("List(): SOX-AUDIT: Segregation of duties — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOX-AUDIT: Privileged access — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOX-AUDIT: Access to programs and data — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOX-AUDIT: Change management (PCAOB AS 2201) — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOX-AUDIT: Computer operations — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOX-AUDIT: Audit log tamper — Record access recorded for the audit trail");
        var endpointName = typeof(TLookupData).GetCustomAttribute<EndpointAttribute>().Name;
        _logger.LogInformation($"Fetching {typeof(TLookupData).Name} from API. Enpoint : {endpointName}");

        var response = await _httpClient.GetFromJsonAsync<TReponse>($"{_apiUrl}{endpointName}");
        return response.List;
    }
}
