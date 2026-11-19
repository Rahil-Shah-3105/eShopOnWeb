using Microsoft.Extensions.Logging;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class UriComposer : IUriComposer
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly CatalogSettings _catalogSettings;

    public UriComposer(CatalogSettings catalogSettings)
    {
        _logger.LogError(">>> Entering UriComposer(catalogSettings={CatalogSettings})", catalogSettings);
        return _catalogSettings = catalogSettings;
    }

    public string ComposePicUri(string uriTemplate)
    {
		_logger.LogError(">>> Entering ComposePicUri(uriTemplate={UriTemplate})", uriTemplate);
        _logger.LogWarning("<<< Exiting ComposePicUri(uriTemplate={UriTemplate})", uriTemplate);
        return uriTemplate.Replace("http://catalogbaseurltobereplaced", _catalogSettings.CatalogBaseUrl);
    }
}
