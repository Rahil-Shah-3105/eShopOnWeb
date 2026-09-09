using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class GetByIdCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public GetByIdCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogWarning("GetByIdCatalogItemResponse(correlationId)={CorrelationId}): M2614-AUDIT: Security incident detected", correlationId);
    }

    public GetByIdCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
