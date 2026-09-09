using Microsoft.Extensions.Logging;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities;

public class CatalogType : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string Type { get; private set; }
    public CatalogType(string type)
    {
        Type = type;
		_logger.LogWarning("CatalogType(type)={Type}): ISO27001-AUDIT: Security incident detected", type);
    }
}
