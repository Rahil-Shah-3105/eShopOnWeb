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
		_logger.LogDebug(">>> Entering HttpService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},toastService={ToastService})", httpClient, baseUrlConfiguration, toastService);
        _httpClient = httpClient;
        _toastService = toastService;
		_logger.LogDebug("HttpService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},toastService={ToastService}): _toastService → {_toastService}", httpClient, baseUrlConfiguration, toastService, _toastService);
		_logger.LogDebug("HttpService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},toastService={ToastService}): _httpClient → {_httpClient}", httpClient, baseUrlConfiguration, toastService, _httpClient);
        _apiUrl = baseUrlConfiguration.Value.ApiBase;
		_logger.LogDebug("HttpService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},toastService={ToastService}): _apiUrl → {_apiUrl}", httpClient, baseUrlConfiguration, toastService, _apiUrl);
		_logger.LogDebug("<<< Exiting HttpService(httpClient={HttpClient},baseUrlConfiguration={BaseUrlConfiguration},toastService={ToastService})", httpClient, baseUrlConfiguration, toastService);
    }

    public async Task<T> HttpGet<T>(string uri)
        where T : class
    {
		_logger.LogDebug(">>> Entering HttpGet(uri={Uri})", uri);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var result = await _httpClient.GetAsync($"{_apiUrl}{uri}");
        if (!result.IsSuccessStatusCode)
		_logger.LogInformation("HttpGet(uri)={Uri}: external service call {Uri1} took {Arg2} ms", uri, uri, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("HttpGet(uri={Uri}): result → {Result}", uri, result);
        {
            return null;
        }

		_logger.LogDebug("<<< Exiting HttpGet(uri={Uri})", uri);
        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T> HttpDelete<T>(string uri, int id)
        where T : class
    {
		_logger.LogDebug(">>> Entering HttpDelete(uri={Uri},id={Id})", uri, id);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var result = await _httpClient.DeleteAsync($"{_apiUrl}{uri}/{id}");
        if (!result.IsSuccessStatusCode)
		_logger.LogInformation("HttpDelete(uri,id)={Uri},{Id}: external service call {Uri1} took {Arg3} ms", uri, id, uri, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("HttpDelete(uri={Uri},id={Id}): result → {Result}", uri, id, result);
        {
            return null;
        }

		_logger.LogDebug("<<< Exiting HttpDelete(uri={Uri},id={Id})", uri, id);
        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T> HttpPost<T>(string uri, object dataToSend)
        where T : class
    {
		_logger.LogDebug(">>> Entering HttpPost(uri={Uri},dataToSend={DataToSend})", uri, dataToSend);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var content = ToJson(dataToSend);

		_logger.LogDebug("HttpPost(uri={Uri},dataToSend={DataToSend}): content → {Content}", uri, dataToSend, content);
		_logger.LogInformation("HttpPost(uri,dataToSend)={Uri},{DataToSend}: external service call {Uri1} took {Arg3} ms", uri, dataToSend, uri, __logStart.ElapsedMilliseconds);
        var result = await _httpClient.PostAsync($"{_apiUrl}{uri}", content);
		_logger.LogDebug("HttpPost(uri={Uri},dataToSend={DataToSend}): result → {Result}", uri, dataToSend, result);
        if (!result.IsSuccessStatusCode)
        {
            var exception = JsonSerializer.Deserialize<ErrorDetails>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            _toastService.ShowToast($"Error : {exception.Message}", ToastLevel.Error);

            return null;
        }

		_logger.LogDebug("<<< Exiting HttpPost(uri={Uri},dataToSend={DataToSend})", uri, dataToSend);
        return await FromHttpResponseMessage<T>(result);
    }

    public async Task<T> HttpPut<T>(string uri, object dataToSend)
        where T : class
    {
		_logger.LogDebug(">>> Entering HttpPut(uri={Uri},dataToSend={DataToSend})", uri, dataToSend);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var content = ToJson(dataToSend);

		_logger.LogDebug("HttpPut(uri={Uri},dataToSend={DataToSend}): content → {Content}", uri, dataToSend, content);
		_logger.LogInformation("HttpPut(uri,dataToSend)={Uri},{DataToSend}: external service call {Uri1} took {Arg3} ms", uri, dataToSend, uri, __logStart.ElapsedMilliseconds);
        var result = await _httpClient.PutAsync($"{_apiUrl}{uri}", content);
		_logger.LogDebug("HttpPut(uri={Uri},dataToSend={DataToSend}): result → {Result}", uri, dataToSend, result);
        if (!result.IsSuccessStatusCode)
        {
            _toastService.ShowToast("Error", ToastLevel.Error);
            return null;
        }

		_logger.LogDebug("<<< Exiting HttpPut(uri={Uri},dataToSend={DataToSend})", uri, dataToSend);
        return await FromHttpResponseMessage<T>(result);
    }

    private StringContent ToJson(object obj)
    {
		_logger.LogDebug(">>> Entering ToJson(obj={Obj})", obj);
		_logger.LogDebug("<<< Exiting ToJson(obj={Obj})", obj);
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    private async Task<T> FromHttpResponseMessage<T>(HttpResponseMessage result)
    {
		_logger.LogDebug(">>> Entering FromHttpResponseMessage(result={Result})", result);
        return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
			_logger.LogDebug("<<< Exiting FromHttpResponseMessage(result={Result})", result);
            PropertyNameCaseInsensitive = true
        });
    }
}
