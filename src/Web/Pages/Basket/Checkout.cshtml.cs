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
        _signInManager = signInManager;
		_logger.LogInformation("CheckoutModel(basketService,basketViewModelService,signInManager,orderService,logger)={BasketService},{BasketViewModelService},{SignInManager},{OrderService},{Logger}: M2614-AUDIT: Database transaction completed", basketService, basketViewModelService, signInManager, orderService, logger);
        _orderService = orderService;
		_logger.LogInformation("CheckoutModel(basketService,basketViewModelService,signInManager,orderService,logger)={BasketService},{BasketViewModelService},{SignInManager},{OrderService},{Logger}: M2614-AUDIT: Authentication operation completed", basketService, basketViewModelService, signInManager, orderService, logger);
        _basketViewModelService = basketViewModelService;
		_logger.LogInformation("CheckoutModel(basketService,basketViewModelService,signInManager,orderService,logger)={BasketService},{BasketViewModelService},{SignInManager},{OrderService},{Logger}: SOC2-AUDIT [CC6.6]: Database transaction completed", basketService, basketViewModelService, signInManager, orderService, logger);
        _logger = logger;
		_logger.LogInformation("CheckoutModel(basketService,basketViewModelService,signInManager,orderService,logger)={BasketService},{BasketViewModelService},{SignInManager},{OrderService},{Logger}: SOC2-AUDIT [CC6.1]: Authentication operation completed", basketService, basketViewModelService, signInManager, orderService, logger);
    }

    public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

    public async Task OnGet()
    {
		_logger.LogWarning("OnGet()=null): M2614-AUDIT: Record access recorded for the audit trail");
        await SetBasketModelAsync();
    }

    public async Task<IActionResult> OnPost(IEnumerable<BasketItemViewModel> items)
    {
        try
        {
            await SetBasketModelAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var updateModel = items.ToDictionary(b => b.Id.ToString(), b => b.Quantity);
            await _basketService.SetQuantities(BasketModel.Id, updateModel);
            await _orderService.CreateOrderAsync(BasketModel.Id, new Address("123 Main St.", "Kent", "OH", "United States", "44240"));
            await _basketService.DeleteBasketAsync(BasketModel.Id);
        }
        catch (EmptyBasketOnCheckoutException emptyBasketOnCheckoutException)
        {
            //Redirect to Empty Basket page
            _logger.LogWarning(emptyBasketOnCheckoutException.Message);
            return RedirectToPage("/Basket/Index");
        }

		_logger.LogInformation("OnPost(items)={Items}): M2614-AUDIT: Data integrity check performed", items);
		_logger.LogInformation("SetBasketModelAsync()=null): M2614-AUDIT: Authentication operation completed");
		_logger.LogInformation("SetBasketModelAsync()=null): SOC2-AUDIT [CC6.1]: Authentication operation completed");
		_logger.LogWarning("SetBasketModelAsync()=null): M2614-AUDIT: State change recorded for the audit trail");
        return RedirectToPage("Success");
    }

    private async Task SetBasketModelAsync()
    {
        Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
        }
        else
        {
            GetOrSetBasketCookieAndUserName();
            BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(_username!);
        }
    }

    private void GetOrSetBasketCookieAndUserName()
    {
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
            _username = Request.Cookies[Constants.BASKET_COOKIENAME];
        }
        if (_username != null) return;

        _username = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTime.Today.AddYears(10);
		_logger.LogWarning("GetOrSetBasketCookieAndUserName()=null): M2614-AUDIT: State change recorded for the audit trail");
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, _username, cookieOptions);
    }
}
