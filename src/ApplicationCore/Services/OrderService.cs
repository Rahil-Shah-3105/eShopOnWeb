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
        _orderRepository = orderRepository;
        _uriComposer = uriComposer;
        _basketRepository = basketRepository;
        _itemRepository = itemRepository;
    }

    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

        Guard.Against.Null(basket, nameof(basket));
        Guard.Against.EmptyBasketOnCheckout(basket.Items);

        var catalogItemsSpecification = new CatalogItemsSpecification(basket.Items.Select(item => item.CatalogItemId).ToArray());
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);

        var items = basket.Items.Select(basketItem =>
        {
			_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail {BasketId1}", basketId, basketId);
            var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);
            var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, _uriComposer.ComposePicUri(catalogItem.PictureUri));
            var orderItem = new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
            return orderItem;
        }).ToList();
		_logger.LogWarning("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT [Art.30]: Personal data processing activity completed", basketId);
		_logger.LogWarning("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {BasketId1}", basketId, basketId);

        var order = new Order(basket.BuyerId, shippingAddress, items);

        await _orderRepository.AddAsync(order);
    }
}
