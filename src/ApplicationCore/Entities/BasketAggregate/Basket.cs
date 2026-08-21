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
		_logger.LogDebug(">>> Entering Basket(buyerId={BuyerId})", buyerId);
        BuyerId = buyerId;
		_logger.LogDebug("Basket(buyerId={BuyerId}): BuyerId → {BuyerId1}", buyerId, BuyerId);
    }

    public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
    {
		_logger.LogDebug(">>> Entering AddItem(catalogItemId={CatalogItemId},unitPrice={UnitPrice},quantity={Quantity})", catalogItemId, unitPrice, quantity);
        if (!Items.Any(i => i.CatalogItemId == catalogItemId))
        {
            _items.Add(new BasketItem(catalogItemId, quantity, unitPrice));
            return;
        }
        var existingItem = Items.First(i => i.CatalogItemId == catalogItemId);
		_logger.LogDebug("AddItem(catalogItemId={CatalogItemId},unitPrice={UnitPrice},quantity={Quantity}): existingItem → {ExistingItem}", catalogItemId, unitPrice, quantity, existingItem);
        existingItem.AddQuantity(quantity);
		_logger.LogDebug("<<< Exiting AddItem(catalogItemId={CatalogItemId},unitPrice={UnitPrice},quantity={Quantity})", catalogItemId, unitPrice, quantity);
    }

    public void RemoveEmptyItems()
    {
		_logger.LogDebug(">>> Entering RemoveEmptyItems()");
        _items.RemoveAll(i => i.Quantity == 0);
		_logger.LogDebug("<<< Exiting RemoveEmptyItems()");
    }

    public void SetNewBuyerId(string buyerId)
    {
		_logger.LogDebug(">>> Entering SetNewBuyerId(buyerId={BuyerId})", buyerId);
        BuyerId = buyerId;
		_logger.LogDebug("SetNewBuyerId(buyerId={BuyerId}): BuyerId → {BuyerId1}", buyerId, BuyerId);
    }
}
