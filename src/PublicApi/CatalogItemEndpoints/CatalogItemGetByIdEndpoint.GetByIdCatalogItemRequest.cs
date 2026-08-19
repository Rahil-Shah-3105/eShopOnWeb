using Microsoft.Extensions.Logging;
﻿namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class GetByIdCatalogItemRequest : BaseRequest
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public int CatalogItemId { get; init; }

    public GetByIdCatalogItemRequest(int catalogItemId)
    {
		_logger.LogDebug(">>> Entering GetByIdCatalogItemRequest(catalogItemId={CatalogItemId})", catalogItemId);
        CatalogItemId = catalogItemId;
		_logger.LogDebug("GetByIdCatalogItemRequest(catalogItemId={CatalogItemId}): CatalogItemId → {CatalogItemId1}", catalogItemId, CatalogItemId);
		_logger.LogDebug("<<< Exiting GetByIdCatalogItemRequest(catalogItemId={CatalogItemId})", catalogItemId);
    }
}
