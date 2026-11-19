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
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", request);
		_logger.LogWarning("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", request);
        var spec = new OrderWithItemsByIdSpec(request.OrderId);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (order == null)
        {
			_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", request);
            return null;
        }
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art.30]: Personal data processing activity completed", request);
		_logger.LogWarning("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", request);

		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", request);
		_logger.LogInformation("Handle(request={Request}, cancellationToken=<redacted>): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", request);
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
