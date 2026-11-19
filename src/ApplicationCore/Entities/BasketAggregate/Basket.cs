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
		_logger.LogWarning("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
        if (!Items.Any(i => i.CatalogItemId == catalogItemId))
        {
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogWarning("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
			_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
            _items.Add(new BasketItem(catalogItemId, quantity, unitPrice));
            return;
        }
        var existingItem = Items.First(i => i.CatalogItemId == catalogItemId);
        existingItem.AddQuantity(quantity);
    }

    public void RemoveEmptyItems()
    {
		_logger.LogWarning("RemoveEmptyItems(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail");
		_logger.LogWarning("RemoveEmptyItems(): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail");
    _logger.LogInformation("RemoveEmptyItems(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail");
    _logger.LogInformation("RemoveEmptyItems(): GDPR-AUDIT: Breach notification — State change recorded for the audit trail");
        _items.RemoveAll(i => i.Quantity == 0);
    }

    public void SetNewBuyerId(string buyerId)
    {
		_logger.LogWarning("SetNewBuyerId(buyerId={BuyerId}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
		_logger.LogWarning("SetNewBuyerId(buyerId={BuyerId}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
    _logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
    _logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
        BuyerId = buyerId;
    }
}
