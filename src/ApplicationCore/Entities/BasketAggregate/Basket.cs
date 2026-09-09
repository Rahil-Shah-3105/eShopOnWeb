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
		_logger.LogInformation("ENTERING: Basket() method");
        BuyerId = buyerId;
    }

    public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
    {
		_logger.LogInformation("ENTERING: AddItem() method");
        if (!Items.Any(i => i.CatalogItemId == catalogItemId))
        {
            _items.Add(new BasketItem(catalogItemId, quantity, unitPrice));
            return;
        }
        var existingItem = Items.First(i => i.CatalogItemId == catalogItemId);
        existingItem.AddQuantity(quantity);
		_logger.LogWarning("AddItem(catalogItemId,unitPrice,quantity)={CatalogItemId},{UnitPrice},{Quantity}: GDPR-AUDIT: State change recorded for the audit trail {CatalogItemId1}", catalogItemId, unitPrice, quantity, catalogItemId);
    }

    public void RemoveEmptyItems()
    {
		_logger.LogInformation("ENTERING: RemoveEmptyItems() method");
        _items.RemoveAll(i => i.Quantity == 0);
    }

    public void SetNewBuyerId(string buyerId)
    {
		_logger.LogInformation("ENTERING: SetNewBuyerId() method");
        BuyerId = buyerId;
    }
}
