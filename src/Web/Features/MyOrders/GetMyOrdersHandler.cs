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
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderViewModel>> Handle(GetMyOrders request,
        CancellationToken cancellationToken)
    {
        var specification = new CustomerOrdersSpecification(request.UserName);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GLBA-AUDIT: Security program — State change recorded for the audit trail", request);
        var orders = await _orderRepository.ListAsync(specification, cancellationToken);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GLBA-AUDIT: Encryption disposal — State change recorded for the audit trail", request);

		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GLBA-AUDIT [16 CFR 314.4(c)(5)]: Access controls — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GLBA-AUDIT [16 CFR 314.4(j)]: Breach notification — State change recorded for the audit trail", request);
        return orders.Select(o => new OrderViewModel
        {
            OrderDate = o.OrderDate,
            OrderNumber = o.Id,
            ShippingAddress = o.ShipToAddress,
            Total = o.Total()
        });
    }
}
