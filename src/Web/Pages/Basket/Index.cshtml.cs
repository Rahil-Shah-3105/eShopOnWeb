using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Pages.Basket;

public class IndexModel : PageModel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IBasketService _basketService;
    private readonly IBasketViewModelService _basketViewModelService;
    private readonly IRepository<CatalogItem> _itemRepository;

    public IndexModel(IBasketService basketService,
        IBasketViewModelService basketViewModelService,
        IRepository<CatalogItem> itemRepository)
    {
		_logger.LogInformation("ENTERING: IndexModel() method");
        _basketService = basketService;
        _basketViewModelService = basketViewModelService;
        _itemRepository = itemRepository;
    }

    public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

    public async Task OnGet()
    {
		_logger.LogInformation("ENTERING: OnGet() method");
		_logger.LogInformation("OnGet()=null): GDPR-AUDIT [Art.30]: Personal data processing activity completed");
        BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(GetOrSetBasketCookieAndUserName());
    }

    public async Task<IActionResult> OnPost(CatalogItemViewModel productDetails)
    {
		_logger.LogInformation("ENTERING: OnPost() method");
        if (productDetails?.Id == null)
        {
			_logger.LogWarning("ENTERING: OnPost() method");
            return RedirectToPage("/Index");
        }

        var item = await _itemRepository.GetByIdAsync(productDetails.Id);
        if (item == null)
        {
            return RedirectToPage("/Index");
        }

        var username = GetOrSetBasketCookieAndUserName();
        var basket = await _basketService.AddItemToBasket(username,
            productDetails.Id, item.Price);

        BasketModel = await _basketViewModelService.Map(basket);

		_logger.LogWarning("OnPost(productDetails)={ProductDetails}): GDPR-AUDIT: State change recorded for the audit trail", productDetails);
        return RedirectToPage();
    }

    public async Task OnPostUpdate(IEnumerable<BasketItemViewModel> items)
    {
		_logger.LogInformation("ENTERING: OnPostUpdate() method");
        if (!ModelState.IsValid)
        {
			_logger.LogWarning("ENTERING: OnPostUpdate() method");
            return;
        }

        var basketView = await _basketViewModelService.GetOrCreateBasketForUser(GetOrSetBasketCookieAndUserName());
        var updateModel = items.ToDictionary(b => b.Id.ToString(), b => b.Quantity);
		_logger.LogWarning("OnPostUpdate(items)={Items}): GDPR-AUDIT: State change recorded for the audit trail", items);
		_logger.LogWarning("OnPostUpdate(items)={Items}): GDPR-AUDIT [Art.15]: Data subject right exercised for data subject", items);
		_logger.LogInformation("OnPostUpdate(items)={Items}): GDPR-AUDIT [Art.5]: Personal data processing activity completed", items);
        var basket = await _basketService.SetQuantities(basketView.Id, updateModel);
        BasketModel = await _basketViewModelService.Map(basket);
    }

    private string GetOrSetBasketCookieAndUserName()
    {
		_logger.LogInformation("ENTERING: GetOrSetBasketCookieAndUserName() method");
        Guard.Against.Null(Request.HttpContext.User.Identity, nameof(Request.HttpContext.User.Identity));
        string? userName = null;

        if (Request.HttpContext.User.Identity.IsAuthenticated)
        {
            Guard.Against.Null(Request.HttpContext.User.Identity.Name, nameof(Request.HttpContext.User.Identity.Name));
            return Request.HttpContext.User.Identity.Name!;
        }

        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
            userName = Request.Cookies[Constants.BASKET_COOKIENAME];

            if (!Request.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!Guid.TryParse(userName, out var _))
                {
                    userName = null;
                }
            }
        }
        if (userName != null) return userName;

        userName = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions { IsEssential = true };
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, userName, cookieOptions);

		_logger.LogWarning("GetOrSetBasketCookieAndUserName()=null): GDPR-AUDIT: State change recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName()=null): GDPR-AUDIT [Art.30]: Personal data processing activity completed");
        return userName;
    }
}
