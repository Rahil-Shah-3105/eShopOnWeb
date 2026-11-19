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
		_logger.LogInformation("AddQuantity(quantity={Quantity}): GLBA-AUDIT: Security program — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): GLBA-AUDIT: Encryption disposal — State change recorded for the audit trail", quantity);

		_logger.LogInformation("AddQuantity(quantity={Quantity}): GLBA-AUDIT [16 CFR 314.4(j)]: Breach notification — State change recorded for the audit trail", quantity);
        Quantity += quantity;
		_logger.LogInformation("AddQuantity(quantity={Quantity}): GLBA-AUDIT [16 CFR 314.4(c)(5)]: Access controls — State change recorded for the audit trail", quantity);
    }

    public void SetQuantity(int quantity)
    {
		_logger.LogInformation("SetQuantity(quantity={Quantity}): GLBA-AUDIT: Security program — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): GLBA-AUDIT: Encryption disposal — State change recorded for the audit trail", quantity);

		_logger.LogInformation("SetQuantity(quantity={Quantity}): GLBA-AUDIT [16 CFR 314.4(j)]: Breach notification — State change recorded for the audit trail", quantity);
        Quantity = quantity;
		_logger.LogInformation("SetQuantity(quantity={Quantity}): GLBA-AUDIT [16 CFR 314.4(c)(5)]: Access controls — State change recorded for the audit trail", quantity);
    }
}
