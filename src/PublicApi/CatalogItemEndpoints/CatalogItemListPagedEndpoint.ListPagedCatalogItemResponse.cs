using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class ListPagedCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ListPagedCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogWarning("ListPagedCatalogItemResponse(correlationId)={CorrelationId}): NERCCIP-AUDIT: Security incident detected", correlationId);
	_logger.LogWarning("ListPagedCatalogItemResponse()=null): NERCCIP-AUDIT: Security incident detected");
    }

    public ListPagedCatalogItemResponse()
    {
    }

    public List<CatalogItemDto> CatalogItems { get; set; } = new List<CatalogItemDto>();
    public int PageCount { get; set; }
}
