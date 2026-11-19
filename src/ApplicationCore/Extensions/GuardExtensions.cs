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
			_logger.LogInformation("EmptyBasketOnCheckout(guardClause={GuardClause}, basketItems={BasketItems}): HIPAA-AUDIT [§164.312(c)(1)]: Data integrity check performed", guardClause, basketItems);
        if (!basketItems.Any()) {
            throw new EmptyBasketOnCheckoutException();
        }
    }
}
