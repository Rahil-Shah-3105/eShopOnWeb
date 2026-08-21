using Microsoft.Extensions.Logging;
﻿using Ardalis.GuardClauses;

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
		_logger.LogDebug(">>> Entering BasketItem(catalogItemId={CatalogItemId},quantity={Quantity},unitPrice={UnitPrice})", catalogItemId, quantity, unitPrice);
        CatalogItemId = catalogItemId;
		_logger.LogDebug("<<< Exiting BasketItem(catalogItemId={CatalogItemId},quantity={Quantity},unitPrice={UnitPrice})", catalogItemId, quantity, unitPrice);
		_logger.LogDebug("BasketItem(catalogItemId={CatalogItemId},quantity={Quantity},unitPrice={UnitPrice}): CatalogItemId → {CatalogItemId1}", catalogItemId, quantity, unitPrice, CatalogItemId);
        UnitPrice = unitPrice;
		_logger.LogDebug("BasketItem(catalogItemId={CatalogItemId},quantity={Quantity},unitPrice={UnitPrice}): UnitPrice → {UnitPrice1}", catalogItemId, quantity, unitPrice, UnitPrice);
        SetQuantity(quantity);
    }

    public void AddQuantity(int quantity)
    {
		_logger.LogDebug(">>> Entering AddQuantity(quantity={Quantity})", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity += quantity;
		_logger.LogDebug("<<< Exiting AddQuantity(quantity={Quantity})", quantity);
    }

    public void SetQuantity(int quantity)
    {
		_logger.LogDebug(">>> Entering SetQuantity(quantity={Quantity})", quantity);
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

		_logger.LogDebug("<<< Exiting SetQuantity(quantity={Quantity})", quantity);
        Quantity = quantity;
		_logger.LogDebug("SetQuantity(quantity={Quantity}): Quantity → {Quantity1}", quantity, Quantity);
    }
}
