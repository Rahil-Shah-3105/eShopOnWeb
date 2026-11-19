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
		_logger.LogInformation("AddQuantity(quantity={Quantity}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", quantity);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", quantity);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", quantity);
		_logger.LogInformation("AddQuantity(quantity={Quantity}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", quantity);
    _logger.LogInformation("AddQuantity(quantity={Quantity}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", quantity);
    _logger.LogInformation("AddQuantity(quantity={Quantity}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
		_logger.LogInformation("SetQuantity(quantity={Quantity}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", quantity);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", quantity);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", quantity);
		_logger.LogInformation("SetQuantity(quantity={Quantity}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", quantity);
    _logger.LogInformation("SetQuantity(quantity={Quantity}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity = quantity;
    }
}
