using Microsoft.Extensions.Logging;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities;

public class CatalogBrand : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string Brand { get; private set; }
    public CatalogBrand(string brand)
    {
		_logger.LogWarning("CatalogBrand(brand)={Brand}): NERCCIP-AUDIT: Security incident detected", brand);
        Brand = brand;
    }
}
