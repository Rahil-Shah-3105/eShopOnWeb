using MediatR;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Features.MyOrders;

public class GetMyOrdersHandler : IRequestHandler<GetMyOrders, IEnumerable<OrderViewModel>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IReadRepository<Order> _orderRepository;

    public GetMyOrdersHandler(IReadRepository<Order> orderRepository)
    {
		_logger.LogError(">>> Entering GetMyOrdersHandler(orderRepository={OrderRepository})", orderRepository);
        _orderRepository = orderRepository;
		_logger.LogDebug("GetMyOrdersHandler(orderRepository={OrderRepository}): _orderRepository → {_orderRepository}", orderRepository, _orderRepository);
    _logger.LogWarning("<<< Exiting GetMyOrdersHandler(orderRepository={OrderRepository})", orderRepository);
    }

    public async Task<IEnumerable<OrderViewModel>> Handle(GetMyOrders request,
        CancellationToken cancellationToken)
    {
		_logger.LogError(">>> Entering Handle(request={Request},cancellationToken=<redacted>)", request);
        var specification = new CustomerOrdersSpecification(request.UserName);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
		_logger.LogError("Handle(request,cancellationToken)={Request},<redacted>: database query executed in {Arg2} ms", request, __logStart.ElapsedMilliseconds);
        var orders = await _orderRepository.ListAsync(specification, cancellationToken);

		_logger.LogError("Handle(request={Request},cancellationToken=<redacted>): specification → {Specification}", request, specification);
		_logger.LogError("Handle(request={Request},cancellationToken=<redacted>): orders → {Orders}", request, orders);
		_logger.LogWarning("<<< Exiting Handle(request={Request},cancellationToken=<redacted>)", request);
        return orders.Select(o => new OrderViewModel
        {
            OrderDate = o.OrderDate,
            OrderNumber = o.Id,
            ShippingAddress = o.ShipToAddress,
            Total = o.Total()
        });
    }
}
