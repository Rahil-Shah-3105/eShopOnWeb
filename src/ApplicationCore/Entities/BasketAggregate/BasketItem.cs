using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;

public class BasketItem : BaseEntity
{
private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;

    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public int CatalogItemId { get; private set; }
    public int BasketId { get; private set; }

    public BasketItem(int catalogItemId, int quantity, decimal unitPrice)
    {
		_logger.LogInformation("ENTERING: BasketItem() method");
        CatalogItemId = catalogItemId;
        UnitPrice = unitPrice;
        SetQuantity(quantity);
    }

    public void AddQuantity(int quantity)
    {
		_logger.LogInformation("ENTERING: AddQuantity() method");
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

		_logger.LogInformation("AddQuantity(quantity)={Quantity}): GDPR-AUDIT [Art.30]: Personal data processing activity completed", quantity);
        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
		_logger.LogInformation("ENTERING: SetQuantity() method");
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

		_logger.LogWarning("SetQuantity(quantity)={Quantity}): GDPR-AUDIT: State change recorded for the audit trail", quantity);
        Quantity = quantity;
    }
}
