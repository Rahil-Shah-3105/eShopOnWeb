using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class DeleteCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public DeleteCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogWarning("DeleteCatalogItemResponse(correlationId)={CorrelationId}): NERCCIP-AUDIT: Security incident detected", correlationId);
	_logger.LogWarning("DeleteCatalogItemResponse()=null): NERCCIP-AUDIT: Security incident detected");
    }

    public DeleteCatalogItemResponse()
    {
    }

    public string Status { get; set; } = "Deleted";
}
