using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;

public class Basket : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string BuyerId { get; private set; }
    private readonly List<BasketItem> _items = new List<BasketItem>();
    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

    public int TotalItems => _items.Sum(i => i.Quantity);


    public Basket(string buyerId)
    {
        BuyerId = buyerId;
    }

    public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
    {
		_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
        if (!Items.Any(i => i.CatalogItemId == catalogItemId))
        {
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
            _items.Add(new BasketItem(catalogItemId, quantity, unitPrice));
            return;
        }
        var existingItem = Items.First(i => i.CatalogItemId == catalogItemId);
        existingItem.AddQuantity(quantity);
    }

    public void RemoveEmptyItems()
    {
		_logger.LogInformation("RemoveEmptyItems(): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail");
		_logger.LogInformation("RemoveEmptyItems(): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail");
    _logger.LogInformation("RemoveEmptyItems(): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail");
    _logger.LogInformation("RemoveEmptyItems(): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail");
        _items.RemoveAll(i => i.Quantity == 0);
    }

    public void SetNewBuyerId(string buyerId)
    {
		_logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
		_logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
    _logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
        BuyerId = buyerId;
    }
}
