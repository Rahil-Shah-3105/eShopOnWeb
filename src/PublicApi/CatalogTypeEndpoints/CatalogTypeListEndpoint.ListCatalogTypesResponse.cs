using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogTypeEndpoints;

public class ListCatalogTypesResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ListCatalogTypesResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogError(">>> Entering ListCatalogTypesResponse(correlationId={CorrelationId})", correlationId);
    _logger.LogWarning("<<< Exiting ListCatalogTypesResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogError(">>> Entering ListCatalogTypesResponse()");
    _logger.LogWarning("<<< Exiting ListCatalogTypesResponse()");
    }

    public ListCatalogTypesResponse()
    {
    }

    public List<CatalogTypeDto> CatalogTypes { get; set; } = new List<CatalogTypeDto>();
}
