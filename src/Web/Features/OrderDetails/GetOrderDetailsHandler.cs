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
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", request);
        var spec = new OrderWithItemsByIdSpec(request.OrderId);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (order == null)
        {
			_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", request);
            return null;
        }
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", request);

		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", request);
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
