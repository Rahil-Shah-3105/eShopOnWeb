using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Interfaces;

namespace Microsoft.eShopWeb.Web.Pages.Basket;

[Authorize]
public class CheckoutModel : PageModel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IBasketService _basketService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IOrderService _orderService;
    private string? _username = null;
    private readonly IBasketViewModelService _basketViewModelService;
    private readonly IAppLogger<CheckoutModel> _logger;

    public CheckoutModel(IBasketService basketService,
        IBasketViewModelService basketViewModelService,
        SignInManager<ApplicationUser> signInManager,
        IOrderService orderService,
        IAppLogger<CheckoutModel> logger)
    {
        _basketService = basketService;
		_logger.LogDebug("CheckoutModel(basketService={BasketService},basketViewModelService={BasketViewModelService},signInManager={SignInManager},orderService={OrderService},logger={Logger}): _basketService → {_basketService}", basketService, basketViewModelService, signInManager, orderService, logger, _basketService);
        _signInManager = signInManager;
		_logger.LogDebug("CheckoutModel(basketService={BasketService},basketViewModelService={BasketViewModelService},signInManager={SignInManager},orderService={OrderService},logger={Logger}): _signInManager → {_signInManager}", basketService, basketViewModelService, signInManager, orderService, logger, _signInManager);
        _orderService = orderService;
		_logger.LogDebug("CheckoutModel(basketService={BasketService},basketViewModelService={BasketViewModelService},signInManager={SignInManager},orderService={OrderService},logger={Logger}): _orderService → {_orderService}", basketService, basketViewModelService, signInManager, orderService, logger, _orderService);
        _basketViewModelService = basketViewModelService;
		_logger.LogDebug("CheckoutModel(basketService={BasketService},basketViewModelService={BasketViewModelService},signInManager={SignInManager},orderService={OrderService},logger={Logger}): _basketViewModelService → {_basketViewModelService}", basketService, basketViewModelService, signInManager, orderService, logger, _basketViewModelService);
        _logger = logger;
		_logger.LogError(">>> Entering CheckoutModel(basketService={BasketService},basketViewModelService={BasketViewModelService},signInManager={SignInManager},orderService={OrderService},logger={Logger})", basketService, basketViewModelService, signInManager, orderService, logger);
    _logger.LogWarning("<<< Exiting CheckoutModel(basketService={BasketService},basketViewModelService={BasketViewModelService},signInManager={SignInManager},orderService={OrderService},logger={Logger})", basketService, basketViewModelService, signInManager, orderService, logger);
    }

    public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

    public async Task OnGet()
    {
		_logger.LogError(">>> Entering OnGet()");
        await SetBasketModelAsync();
		_logger.LogWarning("<<< Exiting OnGet()");
    }

    public async Task<IActionResult> OnPost(IEnumerable<BasketItemViewModel> items)
    {
		_logger.LogError(">>> Entering OnPost(items={Items})", items);
        try
        {
            await SetBasketModelAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var updateModel = items.ToDictionary(b => b.Id.ToString(), b => b.Quantity);
			_logger.LogError("OnPost(items={Items}): updateModel → {UpdateModel}", items, updateModel);
            await _basketService.SetQuantities(BasketModel.Id, updateModel);
            await _orderService.CreateOrderAsync(BasketModel.Id, new Address("123 Main St.", "Kent", "OH", "United States", "44240"));
            await _basketService.DeleteBasketAsync(BasketModel.Id);
        }
        catch (EmptyBasketOnCheckoutException emptyBasketOnCheckoutException)
        {
            //Redirect to Empty Basket page
            _logger.LogWarning(emptyBasketOnCheckoutException.Message);
		_logger.LogWarning("<<< Exiting OnPost(items={Items})", items);
            return RedirectToPage("/Basket/Index");
        }

		_logger.LogError(">>> Entering SetBasketModelAsync()");
		_logger.LogWarning("<<< Exiting SetBasketModelAsync()");
        return RedirectToPage("Success");
    }

    private async Task SetBasketModelAsync()
    {
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
			_logger.LogError("SetBasketModelAsync(): BasketModel → {BasketModel}", BasketModel);
        }
        else
        {
			_logger.LogDebug("SetBasketModelAsync(): BasketModel → {BasketModel}", BasketModel);
            GetOrSetBasketCookieAndUserName();
            BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(_username!);
        }
    }

    private void GetOrSetBasketCookieAndUserName()
    {
		_logger.LogError(">>> Entering GetOrSetBasketCookieAndUserName()");
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
            _username = Request.Cookies[Constants.BASKET_COOKIENAME];
			_logger.LogError("GetOrSetBasketCookieAndUserName(): _username → {_username}", _username);
			_logger.LogDebug("GetOrSetBasketCookieAndUserName(): _username → {_username}", _username);
        }
        if (_username != null) return;

        _username = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions();
		_logger.LogError("GetOrSetBasketCookieAndUserName(): cookieOptions → <redacted>");
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, _username, cookieOptions);
		_logger.LogWarning("<<< Exiting GetOrSetBasketCookieAndUserName()");
    }
}
