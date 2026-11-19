using Microsoft.Extensions.Logging;
namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class DeleteCatalogItemRequest : BaseRequest
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public int CatalogItemId { get; init; }

    public DeleteCatalogItemRequest(int catalogItemId)
    {
		_logger.LogError(">>> Entering DeleteCatalogItemRequest(catalogItemId={CatalogItemId})", catalogItemId);
        CatalogItemId = catalogItemId;
		_logger.LogDebug("DeleteCatalogItemRequest(catalogItemId={CatalogItemId}): CatalogItemId → {CatalogItemId1}", catalogItemId, CatalogItemId);
    _logger.LogWarning("<<< Exiting DeleteCatalogItemRequest(catalogItemId={CatalogItemId})", catalogItemId);
    }
}
