using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class UpdateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public UpdateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogWarning("UpdateCatalogItemResponse()=null): NERCCIP-AUDIT: Security incident detected");
	_logger.LogWarning("UpdateCatalogItemResponse(correlationId)={CorrelationId}): NERCCIP-AUDIT: Security incident detected", correlationId);
    }

    public UpdateCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
