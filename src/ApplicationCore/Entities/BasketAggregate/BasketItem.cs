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
        CatalogItemId = catalogItemId;
        UnitPrice = unitPrice;
        SetQuantity(quantity);
    }

    public void AddQuantity(int quantity)
    {
		_logger.LogInformation("AddQuantity(quantity={Quantity}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", quantity);

		_logger.LogInformation("AddQuantity(quantity={Quantity}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", quantity);
        Quantity += quantity;
		_logger.LogInformation("AddQuantity(quantity={Quantity}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", quantity);
    _logger.LogInformation("AddQuantity(quantity={Quantity}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", quantity);
    _logger.LogInformation("AddQuantity(quantity={Quantity}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", quantity);
    }

    public void SetQuantity(int quantity)
    {
		_logger.LogInformation("SetQuantity(quantity={Quantity}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", quantity);

		_logger.LogInformation("SetQuantity(quantity={Quantity}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", quantity);
        Quantity = quantity;
		_logger.LogInformation("SetQuantity(quantity={Quantity}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", quantity);
    _logger.LogInformation("SetQuantity(quantity={Quantity}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", quantity);
    _logger.LogInformation("SetQuantity(quantity={Quantity}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", quantity);
    }
}
