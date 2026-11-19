using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class CreateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CreateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogError(">>> Entering CreateCatalogItemResponse(correlationId={CorrelationId})", correlationId);
    _logger.LogWarning("<<< Exiting CreateCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogError(">>> Entering CreateCatalogItemResponse()");
    _logger.LogWarning("<<< Exiting CreateCatalogItemResponse()");
    }

    public CreateCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
