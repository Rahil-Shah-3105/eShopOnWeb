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
		_logger.LogInformation("CatalogLookupDataService(httpClient,baseUrlConfiguration,logger)={HttpClient},{BaseUrlConfiguration},{Logger}: M2614-AUDIT: Change management operation performed", httpClient, baseUrlConfiguration, logger);
        _httpClient = httpClient;
        _logger = logger;
		_logger.LogWarning("CatalogLookupDataService(httpClient,baseUrlConfiguration,logger)={HttpClient},{BaseUrlConfiguration},{Logger}: M2614-AUDIT: Security incident detected", httpClient, baseUrlConfiguration, logger);
        _apiUrl = baseUrlConfiguration.Value.ApiBase;
		_logger.LogInformation("CatalogLookupDataService(httpClient,baseUrlConfiguration,logger)={HttpClient},{BaseUrlConfiguration},{Logger}: SOC2-AUDIT [CC8.1]: Change management operation performed", httpClient, baseUrlConfiguration, logger);
    }

    public async Task<List<TLookupData>> List()
    {
        var endpointName = typeof(TLookupData).GetCustomAttribute<EndpointAttribute>().Name;
        _logger.LogInformation($"Fetching {typeof(TLookupData).Name} from API. Enpoint : {endpointName}");
		_logger.LogWarning("List()=null): M2614-AUDIT: Record access recorded for the audit trail");

        var response = await _httpClient.GetFromJsonAsync<TReponse>($"{_apiUrl}{endpointName}");
		_logger.LogInformation("List()=null): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {Response}", response);
        return response.List;
    }
}
