using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogBrandEndpoints;

public class ListCatalogBrandsResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ListCatalogBrandsResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogWarning("ListCatalogBrandsResponse(correlationId)={CorrelationId}): NERCCIP-AUDIT: Security incident detected", correlationId);
	_logger.LogWarning("ListCatalogBrandsResponse()=null): NERCCIP-AUDIT: Security incident detected");
    }

    public ListCatalogBrandsResponse()
    {
    }

    public List<CatalogBrandDto> CatalogBrands { get; set; } = new List<CatalogBrandDto>();
}
