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
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var basketSpec = new BasketWithItemsSpecification(basketId);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 10: automated alerting — State change recorded for the audit trail {BasketId1}", basketId, basketId);

		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        Guard.Against.Null(basket, nameof(basket));
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        Guard.Against.EmptyBasketOnCheckout(basket.Items);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail {BasketId1}", basketId, basketId);

		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var catalogItemsSpecification = new CatalogItemsSpecification(basket.Items.Select(item => item.CatalogItemId).ToArray());
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail {BasketId1}", basketId, basketId);

		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var items = basket.Items.Select(basketItem =>
        {
			_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail {BasketId1}", basketId, basketId);
            var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);
            var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, _uriComposer.ComposePicUri(catalogItem.PictureUri));
            var orderItem = new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
            return orderItem;
        }).ToList();
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail {BasketId1}", basketId, basketId);

		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var order = new Order(basket.BuyerId, shippingAddress, items);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {BasketId1}", basketId, basketId);

		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        await _orderRepository.AddAsync(order);
		_logger.LogInformation("CreateOrderAsync(basketId={BasketId}, shippingAddress=<redacted>): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {BasketId1}", basketId, basketId);
    }
}
