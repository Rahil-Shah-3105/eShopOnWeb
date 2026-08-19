using Microsoft.Extensions.Logging;
﻿using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class UpdateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public UpdateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogDebug(">>> Entering UpdateCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogDebug("<<< Exiting UpdateCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogDebug(">>> Entering UpdateCatalogItemResponse()");
    }

    public UpdateCatalogItemResponse()
    {
	_logger.LogDebug("<<< Exiting UpdateCatalogItemResponse()");
    }

    public CatalogItemDto CatalogItem { get; set; }
}
