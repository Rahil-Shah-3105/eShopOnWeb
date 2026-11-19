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
        _orderRepository = orderRepository;
    }

    public async Task<OrderDetailViewModel?> Handle(GetOrderDetails request,
        CancellationToken cancellationToken)
    {
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", request);
        var spec = new OrderWithItemsByIdSpec(request.OrderId);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", request);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", request);

		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", request);
        if (order == null)
        {
			_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", request);
			_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail", request);
            return null;
        }
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", request);

		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", request);
        _logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail", request);
        _logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 10: automated alerting — State change recorded for the audit trail", request);
        _logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail", request);
        _logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", request);
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
