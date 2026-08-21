using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class CustomerOrdersWithItemsSpecification : Specification<Order>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CustomerOrdersWithItemsSpecification(string buyerId)
    {
		_logger.LogDebug(">>> Entering CustomerOrdersWithItemsSpecification(buyerId={BuyerId})", buyerId);
        Query.Where(o => o.BuyerId == buyerId)
            .Include(o => o.OrderItems)
                .ThenInclude(i => i.ItemOrdered);
				_logger.LogDebug("<<< Exiting CustomerOrdersWithItemsSpecification(buyerId={BuyerId})", buyerId);
    }
}
