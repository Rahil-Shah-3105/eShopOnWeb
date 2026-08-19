using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class ListPagedCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ListPagedCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogDebug(">>> Entering ListPagedCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogDebug("<<< Exiting ListPagedCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogDebug(">>> Entering ListPagedCatalogItemResponse()");
    }

    public ListPagedCatalogItemResponse()
    {
	_logger.LogDebug("<<< Exiting ListPagedCatalogItemResponse()");
    }

    public List<CatalogItemDto> CatalogItems { get; set; } = new List<CatalogItemDto>();
    public int PageCount { get; set; }
}
