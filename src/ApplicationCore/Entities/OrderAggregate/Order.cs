using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

public class Order : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    #pragma warning disable CS8618 // Required by Entity Framework
    private Order() {}

    public Order(string buyerId, Address shipToAddress, List<OrderItem> items)
    {
		_logger.LogError(">>> Entering Order(buyerId={BuyerId},shipToAddress={ShipToAddress},items={Items})", buyerId, shipToAddress, items);
        Guard.Against.NullOrEmpty(buyerId, nameof(buyerId));
		_logger.LogWarning("<<< Exiting Order()");

        BuyerId = buyerId;
		_logger.LogDebug("Order(buyerId={BuyerId},shipToAddress={ShipToAddress},items={Items}): BuyerId → {BuyerId1}", buyerId, shipToAddress, items, BuyerId);
        ShipToAddress = shipToAddress;
		_logger.LogDebug("Order(buyerId={BuyerId},shipToAddress={ShipToAddress},items={Items}): ShipToAddress → {ShipToAddress1}", buyerId, shipToAddress, items, ShipToAddress);
        _orderItems = items;
		_logger.LogDebug("Order(buyerId={BuyerId},shipToAddress={ShipToAddress},items={Items}): _orderItems → {_orderItems}", buyerId, shipToAddress, items, _orderItems);
    }

    public string BuyerId { get; private set; }
    public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;
    public Address ShipToAddress { get; private set; }

    // DDD Patterns comment
    // Using a private collection field, better for DDD Aggregate's encapsulation
    // so OrderItems cannot be added from "outside the AggregateRoot" directly to the collection,
    // but only through the method Order.AddOrderItem() which includes behavior.
    private readonly List<OrderItem> _orderItems = new List<OrderItem>();

    // Using List<>.AsReadOnly() 
    // This will create a read only wrapper around the private list so is protected against "external updates".
    // It's much cheaper than .ToList() because it will not have to copy all items in a new collection. (Just one heap alloc for the wrapper instance)
    //https://msdn.microsoft.com/en-us/library/e78dcd75(v=vs.110).aspx 
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal Total()
    {
		_logger.LogError(">>> Entering Total()");
        var total = 0m;
		_logger.LogError("Total(): total → {Total}", total);
        foreach (var item in _orderItems)
        {
            total += item.UnitPrice * item.Units;
        }
		_logger.LogWarning("<<< Exiting Total()");
        return total;
    }
}
