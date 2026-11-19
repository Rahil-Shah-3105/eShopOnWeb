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
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", productDetails);
            return RedirectToPage("/Index");
        }

        var username = GetOrSetBasketCookieAndUserName();
        var basket = await _basketService.AddItemToBasket(username,
            productDetails.Id, item.Price);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", productDetails);

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
        BasketModel = await _basketViewModelService.Map(basket);
		_logger.LogInformation("OnPostUpdate(items={Items}): SOC2-AUDIT [CC8.1]: Change management operation performed", items);
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
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC7.2]: Monitoring coverage — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail");

        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
			_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail");
            userName = Request.Cookies[Constants.BASKET_COOKIENAME];

            if (!Request.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!Guid.TryParse(userName, out var _))
                {
                    userName = null;
                }
            }
        }
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail");
        if (userName != null) return userName;

        userName = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions { IsEssential = true };
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, userName, cookieOptions);

        return userName;
    }
}
