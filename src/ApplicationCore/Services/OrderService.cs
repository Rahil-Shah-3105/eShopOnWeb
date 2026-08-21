using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class OrderService : IOrderService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IRepository<Order> _orderRepository;
    private readonly IUriComposer _uriComposer;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IRepository<CatalogItem> _itemRepository;

    public OrderService(IRepository<Basket> basketRepository,
        IRepository<CatalogItem> itemRepository,
        IRepository<Order> orderRepository,
        IUriComposer uriComposer)
    {
		_logger.LogDebug(">>> Entering OrderService(basketRepository={BasketRepository},itemRepository={ItemRepository},orderRepository={OrderRepository},uriComposer={UriComposer})", basketRepository, itemRepository, orderRepository, uriComposer);
        _orderRepository = orderRepository;
		_logger.LogDebug("OrderService(basketRepository={BasketRepository},itemRepository={ItemRepository},orderRepository={OrderRepository},uriComposer={UriComposer}): _orderRepository → {_orderRepository}", basketRepository, itemRepository, orderRepository, uriComposer, _orderRepository);
        _uriComposer = uriComposer;
		_logger.LogDebug("OrderService(basketRepository={BasketRepository},itemRepository={ItemRepository},orderRepository={OrderRepository},uriComposer={UriComposer}): _uriComposer → {_uriComposer}", basketRepository, itemRepository, orderRepository, uriComposer, _uriComposer);
        _basketRepository = basketRepository;
		_logger.LogDebug("OrderService(basketRepository={BasketRepository},itemRepository={ItemRepository},orderRepository={OrderRepository},uriComposer={UriComposer}): _basketRepository → {_basketRepository}", basketRepository, itemRepository, orderRepository, uriComposer, _basketRepository);
        _itemRepository = itemRepository;
    }

    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
		_logger.LogDebug(">>> Entering CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>)", basketId);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

		_logger.LogDebug("CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>): basketSpec → {BasketSpec}", basketId, basketSpec);
		_logger.LogInformation("CreateOrderAsync(basketId,shippingAddress)={BasketId},{ShippingAddress}: database query executed in {Arg2} ms", basketId, shippingAddress, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>): basket → {Basket}", basketId, basket);
        Guard.Against.Null(basket, nameof(basket));
        Guard.Against.EmptyBasketOnCheckout(basket.Items);

        var catalogItemsSpecification = new CatalogItemsSpecification(basket.Items.Select(item => item.CatalogItemId).ToArray());
        var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);

		_logger.LogDebug("CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>): catalogItemsSpecification → {CatalogItemsSpecification}", basketId, catalogItemsSpecification);
        var items = basket.Items.Select(basketItem =>
        {
            var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);
			_logger.LogDebug("CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>): catalogItems → {CatalogItems}", basketId, catalogItems);
			_logger.LogDebug("CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>): catalogItem → {CatalogItem}", basketId, catalogItem);
            var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, _uriComposer.ComposePicUri(catalogItem.PictureUri));
            var orderItem = new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
			_logger.LogDebug("CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>): itemOrdered → {ItemOrdered}", basketId, itemOrdered);
			_logger.LogDebug("CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>): orderItem → {OrderItem}", basketId, orderItem);
            return orderItem;
        }).ToList();

        var order = new Order(basket.BuyerId, shippingAddress, items);
		_logger.LogDebug("CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>): order → {Order}", basketId, order);

        await _orderRepository.AddAsync(order);
		_logger.LogDebug("<<< Exiting CreateOrderAsync(basketId={BasketId},shippingAddress=<redacted>)", basketId);
    }
}
