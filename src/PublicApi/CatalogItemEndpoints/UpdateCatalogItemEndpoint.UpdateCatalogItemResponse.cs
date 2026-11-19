using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class UpdateCatalogItemResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public UpdateCatalogItemResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogInformation("UpdateCatalogItemResponse(correlationId={CorrelationId}): SOC2-AUDIT [CC8.1]: Change management operation performed", correlationId);
    _logger.LogInformation("UpdateCatalogItemResponse(correlationId={CorrelationId}): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed", correlationId);
	_logger.LogInformation("UpdateCatalogItemResponse(): SOC2-AUDIT [CC8.1]: Change management operation performed");
    _logger.LogInformation("UpdateCatalogItemResponse(): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed");
    }

    public UpdateCatalogItemResponse()
    {
    }

    public CatalogItemDto CatalogItem { get; set; }
}
