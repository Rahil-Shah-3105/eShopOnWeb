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
		_logger.LogInformation("ENTERING: OrderItem() method");
        ItemOrdered = itemOrdered;
        UnitPrice = unitPrice;
        Units = units;
    }
}
