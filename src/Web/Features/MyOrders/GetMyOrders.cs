using MediatR;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Features.MyOrders;

public class GetMyOrders : IRequest<IEnumerable<OrderViewModel>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string UserName { get; set; }

    public GetMyOrders(string userName)
    {
		_logger.LogInformation("ENTERING: GetMyOrders() method");
        UserName = userName;
    }
}
