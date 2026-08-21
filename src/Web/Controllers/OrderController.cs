using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Web.Features.MyOrders;
using Microsoft.eShopWeb.Web.Features.OrderDetails;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
[Route("[controller]/[action]")]
public class OrderController : Controller
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
		_logger.LogDebug(">>> Entering OrderController(mediator={Mediator})", mediator);
        _mediator = mediator;
		_logger.LogDebug("OrderController(mediator={Mediator}): _mediator → {_mediator}", mediator, _mediator);
    }

    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {   
		_logger.LogDebug(">>> Entering MyOrders()");
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        var viewModel = await _mediator.Send(new GetMyOrders(User.Identity.Name));
		_logger.LogDebug("MyOrders(): viewModel → {ViewModel}", viewModel);

		_logger.LogDebug("<<< Exiting MyOrders()");
        return View(viewModel);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> Detail(int orderId)
    {
		_logger.LogDebug(">>> Entering Detail(orderId={OrderId})", orderId);
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        var viewModel = await _mediator.Send(new GetOrderDetails(User.Identity.Name, orderId));
		_logger.LogDebug("Detail(orderId={OrderId}): viewModel → {ViewModel}", orderId, viewModel);

        if (viewModel == null)
        {
		_logger.LogDebug("<<< Exiting Detail(orderId={OrderId})", orderId);
            return BadRequest("No such order found for this user.");
        }

        return View(viewModel);
    }
}
