using System;
using System.Linq;
using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class CatalogItemsSpecification : Specification<CatalogItem>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CatalogItemsSpecification(params int[] ids)
    {
		_logger.LogWarning("CatalogItemsSpecification(ids)={Ids}): NIS2-AUDIT: Security incident detected", ids);
        Query.Where(c => ids.Contains(c.Id));
    }
}
