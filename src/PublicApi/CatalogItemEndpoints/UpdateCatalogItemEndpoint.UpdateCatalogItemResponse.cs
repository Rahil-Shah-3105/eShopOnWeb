using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class UpdateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public UpdateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogInformation("UpdateCatalogItemResponse(correlationId)={CorrelationId}): SOC2-AUDIT [CC8.1]: Change management operation performed", correlationId);
	_logger.LogWarning("UpdateCatalogItemResponse(correlationId)={CorrelationId}): M2614-AUDIT: Security incident detected", correlationId);
    }

    public UpdateCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
