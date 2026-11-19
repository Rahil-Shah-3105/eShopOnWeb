using MediatR;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Features.OrderDetails;

public class GetOrderDetails : IRequest<OrderDetailViewModel>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string UserName { get; set; }
    public int OrderId { get; set; }

    public GetOrderDetails(string userName, int orderId)
    {
		_logger.LogError(">>> Entering GetOrderDetails(userName={UserName},orderId={OrderId})", userName, orderId);
        UserName = userName;
        OrderId = orderId;
		_logger.LogDebug("GetOrderDetails(userName={UserName},orderId={OrderId}): OrderId → {OrderId1}", userName, orderId, OrderId);
		_logger.LogWarning("<<< Exiting GetOrderDetails(userName={UserName},orderId={OrderId})", userName, orderId);
    }
}
