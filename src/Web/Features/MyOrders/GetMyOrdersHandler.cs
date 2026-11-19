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
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOX-AUDIT: Privileged access — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", request);
        var specification = new CustomerOrdersSpecification(request.UserName);
        var orders = await _orderRepository.ListAsync(specification, cancellationToken);

		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOX-AUDIT: Computer operations — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", request);
        return orders.Select(o => new OrderViewModel
        {
            OrderDate = o.OrderDate,
            OrderNumber = o.Id,
            ShippingAddress = o.ShipToAddress,
            Total = o.Total()
        });
    }
}
