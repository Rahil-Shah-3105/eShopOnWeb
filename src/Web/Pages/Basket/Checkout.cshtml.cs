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
        _logger = logger;
		_logger.LogWarning("CheckoutModel(basketService={BasketService}, basketViewModelService={BasketViewModelService}, signInManager={SignInManager}, orderService={OrderService}, logger={Logger}): GDPR-AUDIT: Access, erasure, portability — Database transaction completed", basketService, basketViewModelService, signInManager, orderService, logger);
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

		_logger.LogInformation("SetBasketModelAsync(): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail");
		_logger.LogWarning("SetBasketModelAsync(): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail");
		_logger.LogInformation("SetBasketModelAsync(): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail");
		_logger.LogWarning("SetBasketModelAsync(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail");
		_logger.LogInformation("SetBasketModelAsync(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail");
		_logger.LogInformation("SetBasketModelAsync(): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail");
		_logger.LogInformation("SetBasketModelAsync(): GDPR-AUDIT: Breach notification — State change recorded for the audit trail");
		_logger.LogInformation("SetBasketModelAsync(): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail");
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
		_logger.LogWarning("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail");
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
			_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail");
            _username = Request.Cookies[Constants.BASKET_COOKIENAME];
        }
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail");
        if (_username != null) return;

		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art.30]: Personal data processing activity completed");
		_logger.LogWarning("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail");
        _username = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, _username, cookieOptions);
    }
}
