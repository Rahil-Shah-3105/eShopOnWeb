using Microsoft.Extensions.Logging;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities;

public class CatalogType : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string Type { get; private set; }
    public CatalogType(string type)
    {
		_logger.LogError(">>> Entering CatalogType(type={Type})", type);
        Type = type;
		_logger.LogDebug("CatalogType(type={Type}): Type → {Type1}", type, Type);
    _logger.LogWarning("<<< Exiting CatalogType(type={Type})", type);
    }
}
