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
        _orderService = orderService;
        _basketViewModelService = basketViewModelService;
		_logger.LogInformation("CheckoutModel(basketService={BasketService}, basketViewModelService={BasketViewModelService}, signInManager={SignInManager}, orderService={OrderService}, logger={Logger}): FERPA-AUDIT: Inspect amend — Database transaction completed", basketService, basketViewModelService, signInManager, orderService, logger);
        _logger = logger;
		_logger.LogInformation("CheckoutModel(basketService={BasketService}, basketViewModelService={BasketViewModelService}, signInManager={SignInManager}, orderService={OrderService}, logger={Logger}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed", basketService, basketViewModelService, signInManager, orderService, logger);
    }

    public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

    public async Task OnGet()
    {
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

		_logger.LogInformation("SetBasketModelAsync(): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail");
		_logger.LogInformation("SetBasketModelAsync(): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail");
		_logger.LogInformation("SetBasketModelAsync(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail");
		_logger.LogInformation("SetBasketModelAsync(): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail");
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
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FERPA-AUDIT: Inspect amend — Record access recorded for the audit trail");
        if (_username != null) return;

        _username = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions();
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FERPA-AUDIT [34 CFR 99.37]: Directory information — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FERPA-AUDIT [34 CFR 99.30]: Consent — Record access recorded for the audit trail");
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, _username, cookieOptions);
    }
}
