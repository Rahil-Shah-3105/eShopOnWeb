using Microsoft.Extensions.Logging;
namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CatalogItemOrdered ItemOrdered { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Units { get; private set; }

    #pragma warning disable CS8618 // Required by Entity Framework
    private OrderItem() {}

    public OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
    {
		_logger.LogError(">>> Entering OrderItem(itemOrdered={ItemOrdered},unitPrice={UnitPrice},units={Units})", itemOrdered, unitPrice, units);
        ItemOrdered = itemOrdered;
		_logger.LogWarning("<<< Exiting OrderItem()");
        UnitPrice = unitPrice;
		_logger.LogDebug("OrderItem(itemOrdered={ItemOrdered},unitPrice={UnitPrice},units={Units}): UnitPrice → {UnitPrice1}", itemOrdered, unitPrice, units, UnitPrice);
        Units = units;
		_logger.LogDebug("OrderItem(itemOrdered={ItemOrdered},unitPrice={UnitPrice},units={Units}): Units → {Units1}", itemOrdered, unitPrice, units, Units);
    _logger.LogWarning("<<< Exiting OrderItem(itemOrdered={ItemOrdered},unitPrice={UnitPrice},units={Units})", itemOrdered, unitPrice, units);
    }
}
