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
		_logger.LogDebug(">>> Entering GetOrderDetails(userName={UserName},orderId={OrderId})", userName, orderId);
        UserName = userName;
		_logger.LogDebug("GetOrderDetails(userName={UserName},orderId={OrderId}): UserName → {UserName1}", userName, orderId, UserName);
        OrderId = orderId;
    }
}
