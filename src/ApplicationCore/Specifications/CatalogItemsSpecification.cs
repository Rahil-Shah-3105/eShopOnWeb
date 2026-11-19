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
		_logger.LogError(">>> Entering CatalogItemsSpecification(ids={Ids})", ids);
        Query.Where(c => ids.Contains(c.Id));
		_logger.LogWarning("<<< Exiting CatalogItemsSpecification(ids={Ids})", ids);
    }
}
