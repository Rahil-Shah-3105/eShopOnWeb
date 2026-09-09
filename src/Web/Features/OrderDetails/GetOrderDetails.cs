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
		_logger.LogInformation("ENTERING: GetOrderDetails() method");
        UserName = userName;
        OrderId = orderId;
    }
}
