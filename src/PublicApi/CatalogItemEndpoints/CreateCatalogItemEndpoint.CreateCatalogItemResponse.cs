using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class CreateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CreateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogWarning("CreateCatalogItemResponse(correlationId)={CorrelationId}): SOC2-AUDIT [CC7.3]: Security incident detected", correlationId);
	_logger.LogWarning("CreateCatalogItemResponse()=null): SOC2-AUDIT [CC7.3]: Security incident detected");
    }

    public CreateCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
