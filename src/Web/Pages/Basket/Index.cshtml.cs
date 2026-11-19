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
		_logger.LogInformation("OnGet(): GDPR-AUDIT [Art.30]: Personal data processing activity completed");
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
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", productDetails);
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", productDetails);
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", productDetails);

        var username = GetOrSetBasketCookieAndUserName();
        var basket = await _basketService.AddItemToBasket(username,
            productDetails.Id, item.Price);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", productDetails);
			_logger.LogWarning("OnPost(productDetails={ProductDetails}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", productDetails);
			_logger.LogWarning("OnPost(productDetails={ProductDetails}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", productDetails);

        BasketModel = await _basketViewModelService.Map(basket);

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
		_logger.LogWarning("OnPostUpdate(items={Items}): GDPR-AUDIT [Art.15]: Data subject right exercised for data subject", items);
		_logger.LogInformation("OnPostUpdate(items={Items}): GDPR-AUDIT [Art.5]: Personal data processing activity completed", items);
        BasketModel = await _basketViewModelService.Map(basket);
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
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT: Transfers to third countries — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail");

        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
			_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail");
            userName = Request.Cookies[Constants.BASKET_COOKIENAME];

            if (!Request.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!Guid.TryParse(userName, out var _))
                {
                    userName = null;
                }
            }
        }
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail");
        if (userName != null) return userName;

        userName = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions { IsEssential = true };
		_logger.LogWarning("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art.30]: Personal data processing activity completed");
		_logger.LogWarning("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail");
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, userName, cookieOptions);

        return userName;
    }
}
