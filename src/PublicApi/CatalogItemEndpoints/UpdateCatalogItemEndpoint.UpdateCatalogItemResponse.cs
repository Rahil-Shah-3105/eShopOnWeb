using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class UpdateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public UpdateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogWarning("UpdateCatalogItemResponse(correlationId)={CorrelationId}): SOC2-AUDIT [CC7.3]: Security incident detected", correlationId);
	_logger.LogWarning("UpdateCatalogItemResponse()=null): SOC2-AUDIT [CC7.3]: Security incident detected");
    }

    public UpdateCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
