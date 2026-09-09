using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class UpdateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public UpdateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogInformation("UpdateCatalogItemResponse(correlationId)={CorrelationId}): FEDRAMP-AUDIT: Change management operation performed", correlationId);
	_logger.LogInformation("UpdateCatalogItemResponse()=null): FEDRAMP-AUDIT: Change management operation performed");
    }

    public UpdateCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
