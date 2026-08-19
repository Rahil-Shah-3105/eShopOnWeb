using Microsoft.Extensions.Logging;
﻿using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class GetByIdCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public GetByIdCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogDebug(">>> Entering GetByIdCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogDebug("<<< Exiting GetByIdCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogDebug(">>> Entering GetByIdCatalogItemResponse()");
    }

    public GetByIdCatalogItemResponse()
    {
	_logger.LogDebug("<<< Exiting GetByIdCatalogItemResponse()");
    }

    public CatalogItemDto CatalogItem { get; set; }
}
