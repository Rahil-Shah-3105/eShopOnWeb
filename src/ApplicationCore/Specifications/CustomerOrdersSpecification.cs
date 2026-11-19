using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class CustomerOrdersSpecification : Specification<Order>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CustomerOrdersSpecification(string buyerId)
    {
		_logger.LogError(">>> Entering CustomerOrdersSpecification(buyerId={BuyerId})", buyerId);
        Query.Where(o => o.BuyerId == buyerId)
            .Include(o => o.OrderItems);
			_logger.LogWarning("<<< Exiting CustomerOrdersSpecification(buyerId={BuyerId})", buyerId);
    }
}
