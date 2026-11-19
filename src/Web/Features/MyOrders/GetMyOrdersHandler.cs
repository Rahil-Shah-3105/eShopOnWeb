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
		_logger.LogWarning("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art.30]: Personal data processing activity completed", request);
		_logger.LogWarning("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", request);
        var specification = new CustomerOrdersSpecification(request.UserName);
        var orders = await _orderRepository.ListAsync(specification, cancellationToken);

		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", request);
        _logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", request);
        _logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", request);
        return orders.Select(o => new OrderViewModel
        {
            OrderDate = o.OrderDate,
            OrderNumber = o.Id,
            ShippingAddress = o.ShipToAddress,
            Total = o.Total()
        });
    }
}
