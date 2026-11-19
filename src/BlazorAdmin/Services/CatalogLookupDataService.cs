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
		_logger.LogDebug("CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger}): _httpClient → {_httpClient}", httpClient, baseUrlConfiguration, logger, _httpClient);
        _logger = logger;
		_logger.LogError(">>> Entering CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger})", httpClient, baseUrlConfiguration, logger);
		_logger.LogDebug("CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger}): _logger → {_logger}", httpClient, baseUrlConfiguration, logger, _logger);
    _logger.LogWarning("<<< Exiting CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger})", httpClient, baseUrlConfiguration, logger);
        _apiUrl = baseUrlConfiguration.Value.ApiBase;
		_logger.LogDebug("CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger}): _apiUrl → {_apiUrl}", httpClient, baseUrlConfiguration, logger, _apiUrl);
    }

    public async Task<List<TLookupData>> List()
    {
		_logger.LogError(">>> Entering List()");
        var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var endpointName = typeof(TLookupData).GetCustomAttribute<EndpointAttribute>().Name;
        _logger.LogInformation($"Fetching {typeof(TLookupData).Name} from API. Enpoint : {endpointName}");

        var response = await _httpClient.GetFromJsonAsync<TReponse>($"{_apiUrl}{endpointName}");
		_logger.LogError("List(): response → {Response}", response);
		_logger.LogError("List(): external service call {EndpointName} took {Arg1} ms", endpointName, __logStart.ElapsedMilliseconds);
		_logger.LogWarning("<<< Exiting List()");
        return response.List;
    }
}
