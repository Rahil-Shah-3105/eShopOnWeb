using MediatR;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Features.OrderDetails;

public class GetOrderDetailsHandler : IRequestHandler<GetOrderDetails, OrderDetailViewModel?>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IReadRepository<Order> _orderRepository;

    public GetOrderDetailsHandler(IReadRepository<Order> orderRepository)
    {
		_logger.LogError(">>> Entering GetOrderDetailsHandler(orderRepository={OrderRepository})", orderRepository);
        _orderRepository = orderRepository;
		_logger.LogDebug("GetOrderDetailsHandler(orderRepository={OrderRepository}): _orderRepository → {_orderRepository}", orderRepository, _orderRepository);
    _logger.LogWarning("<<< Exiting GetOrderDetailsHandler(orderRepository={OrderRepository})", orderRepository);
    }

    public async Task<OrderDetailViewModel?> Handle(GetOrderDetails request,
        CancellationToken cancellationToken)
    {
		_logger.LogError(">>> Entering Handle(request={Request},cancellationToken=<redacted>)", request);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var spec = new OrderWithItemsByIdSpec(request.OrderId);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);

		_logger.LogError("Handle(request,cancellationToken)={Request},<redacted>: database query executed in {Arg2} ms", request, __logStart.ElapsedMilliseconds);
        if (order == null)
        {
			_logger.LogError("Handle(request={Request},cancellationToken=<redacted>): spec → {Spec}", request, spec);
			_logger.LogError("Handle(request={Request},cancellationToken=<redacted>): order → {Order}", request, order);
            return null;
        }

		_logger.LogWarning("<<< Exiting Handle(request={Request},cancellationToken=<redacted>)", request);
        return new OrderDetailViewModel
        {
            OrderDate = order.OrderDate,
            OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
            {
                PictureUrl = oi.ItemOrdered.PictureUri,
                ProductId = oi.ItemOrdered.CatalogItemId,
                ProductName = oi.ItemOrdered.ProductName,
                UnitPrice = oi.UnitPrice,
                Units = oi.Units
            }).ToList(),
            OrderNumber = order.Id,
            ShippingAddress = order.ShipToAddress,
            Total = order.Total()
        };
    }
}
