using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class UpdateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public UpdateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogError(">>> Entering UpdateCatalogItemResponse(correlationId={CorrelationId})", correlationId);
    _logger.LogWarning("<<< Exiting UpdateCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogError(">>> Entering UpdateCatalogItemResponse()");
    _logger.LogWarning("<<< Exiting UpdateCatalogItemResponse()");
    }

    public UpdateCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
