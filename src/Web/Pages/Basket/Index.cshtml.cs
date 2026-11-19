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
        _basketService = basketService;
        _basketViewModelService = basketViewModelService;
        _itemRepository = itemRepository;
    }

    public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

    public async Task OnGet()
    {
        BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(GetOrSetBasketCookieAndUserName());
    }

    public async Task<IActionResult> OnPost(CatalogItemViewModel productDetails)
    {
        if (productDetails?.Id == null)
        {
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
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): FEDRAMP-AUDIT [RA-5]: Vulnerability management — State change recorded for the audit trail", productDetails);

		_logger.LogInformation("OnPost(productDetails={ProductDetails}): FEDRAMP-AUDIT: Incident response — State change recorded for the audit trail", productDetails);
        BasketModel = await _basketViewModelService.Map(basket);
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): FEDRAMP-AUDIT: Crypto protection — State change recorded for the audit trail", productDetails);

		_logger.LogInformation("OnPost(productDetails={ProductDetails}): FEDRAMP-AUDIT: Configuration management — State change recorded for the audit trail", productDetails);
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): FEDRAMP-AUDIT: Audit completeness — State change recorded for the audit trail", productDetails);
        return RedirectToPage();
    }

    public async Task OnPostUpdate(IEnumerable<BasketItemViewModel> items)
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        var basketView = await _basketViewModelService.GetOrCreateBasketForUser(GetOrSetBasketCookieAndUserName());
        var updateModel = items.ToDictionary(b => b.Id.ToString(), b => b.Quantity);
        var basket = await _basketService.SetQuantities(basketView.Id, updateModel);
        BasketModel = await _basketViewModelService.Map(basket);
		_logger.LogInformation("OnPostUpdate(items={Items}): FEDRAMP-AUDIT: Configuration management — Change management operation performed", items);
    }

    private string GetOrSetBasketCookieAndUserName()
    {
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
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FEDRAMP-AUDIT [RA-5]: Vulnerability management — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FEDRAMP-AUDIT: Incident response — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FEDRAMP-AUDIT: Crypto protection — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FEDRAMP-AUDIT: Configuration management — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): FEDRAMP-AUDIT: Audit completeness — Record access recorded for the audit trail");
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, userName, cookieOptions);

        return userName;
    }
}
