using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public sealed class BasketWithItemsSpecification : Specification<Basket>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public BasketWithItemsSpecification(int basketId)
    {
		_logger.LogError(">>> Entering BasketWithItemsSpecification(basketId={BasketId})", basketId);
        Query
            .Where(b => b.Id == basketId)
            .Include(b => b.Items);
			_logger.LogWarning("<<< Exiting BasketWithItemsSpecification(basketId={BasketId})", basketId);
    }

    public BasketWithItemsSpecification(string buyerId)
    {
		_logger.LogError(">>> Entering BasketWithItemsSpecification(buyerId={BuyerId})", buyerId);
        Query
            .Where(b => b.BuyerId == buyerId)
            .Include(b => b.Items);
			_logger.LogWarning("<<< Exiting BasketWithItemsSpecification(buyerId={BuyerId})", buyerId);
    }
}
