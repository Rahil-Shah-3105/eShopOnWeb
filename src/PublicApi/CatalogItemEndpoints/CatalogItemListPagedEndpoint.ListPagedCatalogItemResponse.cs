using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class ListPagedCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ListPagedCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogError(">>> Entering ListPagedCatalogItemResponse(correlationId={CorrelationId})", correlationId);
    _logger.LogWarning("<<< Exiting ListPagedCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogError(">>> Entering ListPagedCatalogItemResponse()");
    _logger.LogWarning("<<< Exiting ListPagedCatalogItemResponse()");
    }

    public ListPagedCatalogItemResponse()
    {
    }

    public List<CatalogItemDto> CatalogItems { get; set; } = new List<CatalogItemDto>();
    public int PageCount { get; set; }
}
