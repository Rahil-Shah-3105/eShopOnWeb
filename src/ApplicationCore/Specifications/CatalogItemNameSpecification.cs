using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class CatalogItemNameSpecification : Specification<CatalogItem>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CatalogItemNameSpecification(string catalogItemName)
    {
        Query.Where(item => catalogItemName == item.Name);
		_logger.LogWarning("CatalogItemNameSpecification(catalogItemName)={CatalogItemName}): PCI-AUDIT: Security incident detected", catalogItemName);
    }
}
