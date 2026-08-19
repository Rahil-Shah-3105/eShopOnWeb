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
		_logger.LogDebug(">>> Entering CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger})", httpClient, baseUrlConfiguration, logger);
		_logger.LogDebug("CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger}): _logger → {_logger}", httpClient, baseUrlConfiguration, logger, _logger);
        _apiUrl = baseUrlConfiguration.Value.ApiBase;
		_logger.LogDebug("CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger}): _apiUrl → {_apiUrl}", httpClient, baseUrlConfiguration, logger, _apiUrl);
		_logger.LogDebug("<<< Exiting CatalogLookupDataService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},logger={Logger})", httpClient, baseUrlConfiguration, logger);
    }

    public async Task<List<TLookupData>> List()
    {
		_logger.LogDebug(">>> Entering List()");
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var endpointName = typeof(TLookupData).GetCustomAttribute<EndpointAttribute>().Name;
        _logger.LogInformation($"Fetching {typeof(TLookupData).Name} from API. Enpoint : {endpointName}");
		_logger.LogDebug("List(): endpointName → {EndpointName}", endpointName);

        var response = await _httpClient.GetFromJsonAsync<TReponse>($"{_apiUrl}{endpointName}");
		_logger.LogDebug("List(): response → {Response}", response);
		_logger.LogInformation("List(): external service call {EndpointName} took {Arg1} ms", endpointName, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("<<< Exiting List()");
        return response.List;
    }
}
