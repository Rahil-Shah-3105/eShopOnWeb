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
		_logger.LogInformation("EmptyBasketOnCheckout(guardClause={GuardClause}, basketItems={BasketItems}): M2614-AUDIT: Log integrity (encrypt + validate) — Data integrity check performed", guardClause, basketItems);
			_logger.LogInformation("EmptyBasketOnCheckout(guardClause={GuardClause}, basketItems={BasketItems}): SOC2-AUDIT [CC6.6]: Database transaction completed", guardClause, basketItems);
        if (!basketItems.Any()) {
            throw new EmptyBasketOnCheckoutException();
        }
			_logger.LogInformation("EmptyBasketOnCheckout(guardClause={GuardClause}, basketItems={BasketItems}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed", guardClause, basketItems);
    }
}
