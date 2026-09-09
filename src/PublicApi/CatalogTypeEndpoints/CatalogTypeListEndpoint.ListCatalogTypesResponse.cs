using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogTypeEndpoints;

public class ListCatalogTypesResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ListCatalogTypesResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogWarning("ListCatalogTypesResponse(correlationId)={CorrelationId}): FEDRAMP-AUDIT: Security incident detected", correlationId);
	_logger.LogWarning("ListCatalogTypesResponse()=null): FEDRAMP-AUDIT: Security incident detected");
    }

    public ListCatalogTypesResponse()
    {
    }

    public List<CatalogTypeDto> CatalogTypes { get; set; } = new List<CatalogTypeDto>();
}
