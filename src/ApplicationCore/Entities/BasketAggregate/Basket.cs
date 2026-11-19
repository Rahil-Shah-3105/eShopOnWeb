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
		_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
        if (!Items.Any(i => i.CatalogItemId == catalogItemId))
        {
            _items.Add(new BasketItem(catalogItemId, quantity, unitPrice));
            return;
        }
		_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
        var existingItem = Items.First(i => i.CatalogItemId == catalogItemId);
		_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
        existingItem.AddQuantity(quantity);
		_logger.LogInformation("AddItem(catalogItemId={CatalogItemId}, unitPrice={UnitPrice}, quantity={Quantity}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
    }

    public void RemoveEmptyItems()
    {
		_logger.LogInformation("RemoveEmptyItems(): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail");
		_logger.LogInformation("RemoveEmptyItems(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail");
    _logger.LogInformation("RemoveEmptyItems(): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail");
    _logger.LogInformation("RemoveEmptyItems(): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail");
        _items.RemoveAll(i => i.Quantity == 0);
    }

    public void SetNewBuyerId(string buyerId)
    {
		_logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
        BuyerId = buyerId;
		_logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
    _logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
    _logger.LogInformation("SetNewBuyerId(buyerId={BuyerId}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail {BuyerId1}", buyerId, buyerId);
    }
}
