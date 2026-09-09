using Microsoft.Extensions.Logging;
namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class GetByIdCatalogItemRequest : BaseRequest
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public int CatalogItemId { get; init; }

    public GetByIdCatalogItemRequest(int catalogItemId)
    {
        CatalogItemId = catalogItemId;
		_logger.LogWarning("GetByIdCatalogItemRequest(catalogItemId)={CatalogItemId}): SOC2-AUDIT [CC7.3]: Security incident detected", catalogItemId);
    }
}
