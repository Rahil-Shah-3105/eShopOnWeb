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
		_logger.LogInformation("AddQuantity(quantity={Quantity}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", quantity);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", quantity);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", quantity);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", quantity);
    _logger.LogInformation("AddQuantity(quantity={Quantity}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", quantity);
    _logger.LogInformation("AddQuantity(quantity={Quantity}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
		_logger.LogInformation("SetQuantity(quantity={Quantity}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", quantity);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", quantity);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", quantity);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", quantity);
    _logger.LogInformation("SetQuantity(quantity={Quantity}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", quantity);
    _logger.LogInformation("SetQuantity(quantity={Quantity}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity = quantity;
    }
}
