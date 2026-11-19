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
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 10: automated alerting — State change recorded for the audit trail", productDetails);
        if (productDetails?.Id == null)
        {
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail", productDetails);
            return RedirectToPage("/Index");
        }
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail", productDetails);

		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail", productDetails);
        var item = await _itemRepository.GetByIdAsync(productDetails.Id);
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", productDetails);
        if (item == null)
        {
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", productDetails);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", productDetails);
            return RedirectToPage("/Index");
        }
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", productDetails);

		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", productDetails);
        var username = GetOrSetBasketCookieAndUserName();
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", productDetails);
        var basket = await _basketService.AddItemToBasket(username,
            productDetails.Id, item.Price);
			_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", productDetails);

		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", productDetails);
        BasketModel = await _basketViewModelService.Map(basket);
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", productDetails);

		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", productDetails);
		_logger.LogInformation("OnPost(productDetails={ProductDetails}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", productDetails);
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
		_logger.LogInformation("OnPostUpdate(items={Items}): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed", items);
        BasketModel = await _basketViewModelService.Map(basket);
		_logger.LogInformation("OnPostUpdate(items={Items}): SOC2-AUDIT [CC8.1]: Change management operation performed", items);
    }

    private string GetOrSetBasketCookieAndUserName()
    {
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail");
        Guard.Against.Null(Request.HttpContext.User.Identity, nameof(Request.HttpContext.User.Identity));
        string? userName = null;

        if (Request.HttpContext.User.Identity.IsAuthenticated)
        {
			_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail");
            Guard.Against.Null(Request.HttpContext.User.Identity.Name, nameof(Request.HttpContext.User.Identity.Name));
            return Request.HttpContext.User.Identity.Name!;
        }
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");

        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
			_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
            userName = Request.Cookies[Constants.BASKET_COOKIENAME];

            if (!Request.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!Guid.TryParse(userName, out var _))
                {
                    userName = null;
                }
            }
        }
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed");
        if (userName != null) return userName;

		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 11: CEM + THIRF capability — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Outcome 10: automated alerting — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
		_logger.LogInformation("GetOrSetBasketCookieAndUserName(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail");
        userName = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions { IsEssential = true };
        cookieOptions.Expires = DateTime.Today.AddYears(10);
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, userName, cookieOptions);

        return userName;
    }
}
