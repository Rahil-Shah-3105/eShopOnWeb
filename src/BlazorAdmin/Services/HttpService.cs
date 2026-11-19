using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorShared;
using BlazorShared.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Services;

public class HttpService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly HttpClient _httpClient;
    private readonly ToastService _toastService;
    private readonly string _apiUrl;


    public HttpService(HttpClient httpClient, IOptions<BaseUrlConfiguration> baseUrlConfiguration, ToastService toastService)
    {
        _httpClient = httpClient;
        _toastService = toastService;
        _apiUrl = baseUrlConfiguration.Value.ApiBase;
		_logger.LogInformation("HttpService(httpClient={HttpClient}, baseUrlConfiguration={BaseUrlConfiguration}, toastService={ToastService}): SOC2-AUDIT [CC8.1]: Change management operation performed", httpClient, baseUrlConfiguration, toastService);
    }

    public async Task<T> HttpGet<T>(string uri)
        where T : class
    {
        var result = await _httpClient.GetAsync($"{_apiUrl}{uri}");
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

		_logger.LogInformation("HttpGet(uri={Uri}): SOC2-AUDIT [CC7.1]: System operation completed with status {Result}", uri, result);
        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T> HttpDelete<T>(string uri, int id)
        where T : class
    {
        var result = await _httpClient.DeleteAsync($"{_apiUrl}{uri}/{id}");
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

		_logger.LogInformation("HttpDelete(uri={Uri}, id={Id}): SOC2-AUDIT [CC7.1]: System operation completed with status {Result}", uri, id, result);
        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T> HttpPost<T>(string uri, object dataToSend)
        where T : class
    {
        var content = ToJson(dataToSend);

        var result = await _httpClient.PostAsync($"{_apiUrl}{uri}", content);
        if (!result.IsSuccessStatusCode)
        {
            var exception = JsonSerializer.Deserialize<ErrorDetails>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            _toastService.ShowToast($"Error : {exception.Message}", ToastLevel.Error);

            return null;
        }

		_logger.LogInformation("HttpPost(uri={Uri}, dataToSend={DataToSend}): SOC2-AUDIT [CC6.7]: Encryption operation completed with algorithm {Result}", uri, dataToSend, result);
		_logger.LogInformation("HttpPost(uri={Uri}, dataToSend={DataToSend}): SOC2-AUDIT [CC7.1]: System operation completed with status {Result}", uri, dataToSend, result);
        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T> HttpPut<T>(string uri, object dataToSend)
        where T : class
    {
        var content = ToJson(dataToSend);

        var result = await _httpClient.PutAsync($"{_apiUrl}{uri}", content);
        if (!result.IsSuccessStatusCode)
        {
            _toastService.ShowToast("Error", ToastLevel.Error);
            return null;
        }

		_logger.LogInformation("HttpPut(uri={Uri}, dataToSend={DataToSend}): SOC2-AUDIT [CC7.1]: System operation completed with status {Result}", uri, dataToSend, result);
        return await FromHttpResponseMessage<T>(result);
    }

    private StringContent ToJson(object obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    private async Task<T> FromHttpResponseMessage<T>(HttpResponseMessage result)
    {
		_logger.LogInformation("FromHttpResponseMessage(result={Result}): SOC2-AUDIT [CC6.7]: Encryption operation completed with algorithm {Result1}", result, result);
        return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
