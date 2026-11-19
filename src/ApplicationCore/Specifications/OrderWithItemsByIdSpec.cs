using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class OrderWithItemsByIdSpec : Specification<Order>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public OrderWithItemsByIdSpec(int orderId)
    {
		_logger.LogError(">>> Entering OrderWithItemsByIdSpec(orderId={OrderId})", orderId);
        Query
            .Where(order => order.Id == orderId)
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.ItemOrdered);
			_logger.LogWarning("<<< Exiting OrderWithItemsByIdSpec(orderId={OrderId})", orderId);
    }
}
