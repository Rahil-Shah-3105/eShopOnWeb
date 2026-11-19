using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class DeleteCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public DeleteCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogError(">>> Entering DeleteCatalogItemResponse(correlationId={CorrelationId})", correlationId);
    _logger.LogWarning("<<< Exiting DeleteCatalogItemResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogError(">>> Entering DeleteCatalogItemResponse()");
    _logger.LogWarning("<<< Exiting DeleteCatalogItemResponse()");
    }

    public DeleteCatalogItemResponse()
    {
    }

    public string Status { get; set; } = "Deleted";
}
