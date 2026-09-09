using Microsoft.Extensions.Logging;
namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class DeleteCatalogItemRequest : BaseRequest
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public int CatalogItemId { get; init; }

    public DeleteCatalogItemRequest(int catalogItemId)
    {
        CatalogItemId = catalogItemId;
		_logger.LogWarning("DeleteCatalogItemRequest(catalogItemId)={CatalogItemId}): ISO27001-AUDIT: Security incident detected", catalogItemId);
    }
}
