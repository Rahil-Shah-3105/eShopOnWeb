using Microsoft.Extensions.Logging;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities;

public class CatalogBrand : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string Brand { get; private set; }
    public CatalogBrand(string brand)
    {
		_logger.LogError(">>> Entering CatalogBrand(brand={Brand})", brand);
        Brand = brand;
		_logger.LogDebug("CatalogBrand(brand={Brand}): Brand → {Brand1}", brand, Brand);
    _logger.LogWarning("<<< Exiting CatalogBrand(brand={Brand})", brand);
    }
}
