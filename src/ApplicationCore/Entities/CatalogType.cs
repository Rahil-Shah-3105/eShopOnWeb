using Microsoft.Extensions.Logging;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities;

public class CatalogType : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string Type { get; private set; }
    public CatalogType(string type)
    {
		_logger.LogWarning("CatalogType(type)={Type}): NIS2-AUDIT: Security incident detected", type);
        Type = type;
    }
}
