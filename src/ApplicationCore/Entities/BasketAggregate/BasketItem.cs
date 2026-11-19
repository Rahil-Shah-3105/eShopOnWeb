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
		_logger.LogInformation("AddQuantity(quantity={Quantity}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", quantity);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): GDPR-AUDIT [Art.30]: Personal data processing activity completed", quantity);
		_logger.LogWarning("AddQuantity(quantity={Quantity}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", quantity);
		_logger.LogWarning("AddQuantity(quantity={Quantity}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", quantity);
    _logger.LogInformation("AddQuantity(quantity={Quantity}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", quantity);
    _logger.LogInformation("AddQuantity(quantity={Quantity}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
		_logger.LogWarning("SetQuantity(quantity={Quantity}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", quantity);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", quantity);
		_logger.LogWarning("SetQuantity(quantity={Quantity}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", quantity);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", quantity);
    _logger.LogInformation("SetQuantity(quantity={Quantity}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", quantity);
    _logger.LogInformation("SetQuantity(quantity={Quantity}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity = quantity;
    }
}
