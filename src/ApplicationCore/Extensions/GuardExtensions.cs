using System.Collections.Generic;
using System.Linq;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.Extensions.Logging;

namespace Ardalis.GuardClauses;

public static class BasketGuards
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static void EmptyBasketOnCheckout(this IGuardClause guardClause, IReadOnlyCollection<BasketItem> basketItems)
    {
		_logger.LogDebug(">>> Entering EmptyBasketOnCheckout(guardClause={GuardClause},basketItems={BasketItems})", guardClause, basketItems);
        if (!basketItems.Any())
			_logger.LogWarning("Throwing EmptyBasketOnCheckoutException from EmptyBasketOnCheckout(guardClause={GuardClause},basketItems={BasketItems})", guardClause, basketItems);
            throw new EmptyBasketOnCheckoutException();
    }
}
