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
        if (!basketItems.Any())
			_logger.LogInformation("EmptyBasketOnCheckout(guardClause,basketItems)={GuardClause},{BasketItems}: TRAIGA-AUDIT: Database transaction completed", guardClause, basketItems);
            throw new EmptyBasketOnCheckoutException();
    }
}
