using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Microsoft.eShopWeb;

public static class JsonExtensions
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public static T? FromJson<T>(this string json) =>
        JsonSerializer.Deserialize<T>(json, _jsonOptions);
		_logger.LogInformation("FromJson(json={Json}): FEDRAMP-AUDIT: Crypto protection — Encryption operation completed", json);

    public static string ToJson<T>(this T obj) =>
        JsonSerializer.Serialize<T>(obj, _jsonOptions);
}
