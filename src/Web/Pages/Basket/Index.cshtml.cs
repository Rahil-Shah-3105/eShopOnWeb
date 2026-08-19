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
		_logger.LogDebug(">>> Entering IndexModel(basketService={BasketService},basketViewModelService={BasketViewModelService},itemRepository={ItemRepository})", basketService, basketViewModelService, itemRepository);
        _basketService = basketService;
        _basketViewModelService = basketViewModelService;
		_logger.LogDebug("IndexModel(basketService={BasketService},basketViewModelService={BasketViewModelService},itemRepository={ItemRepository}): _basketViewModelService → {_basketViewModelService}", basketService, basketViewModelService, itemRepository, _basketViewModelService);
		_logger.LogDebug("IndexModel(basketService={BasketService},basketViewModelService={BasketViewModelService},itemRepository={ItemRepository}): _basketService → {_basketService}", basketService, basketViewModelService, itemRepository, _basketService);
        _itemRepository = itemRepository;
		_logger.LogDebug("IndexModel(basketService={BasketService},basketViewModelService={BasketViewModelService},itemRepository={ItemRepository}): _itemRepository → {_itemRepository}", basketService, basketViewModelService, itemRepository, _itemRepository);
		_logger.LogDebug("<<< Exiting IndexModel(basketService={BasketService},basketViewModelService={BasketViewModelService},itemRepository={ItemRepository})", basketService, basketViewModelService, itemRepository);
    }

    public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

    public async Task OnGet()
    {
		_logger.LogDebug(">>> Entering OnGet()");
        BasketModel = await _basketViewModelService.GetOrCreateBasketForUser(GetOrSetBasketCookieAndUserName());
		_logger.LogDebug("OnGet(): BasketModel → {BasketModel}", BasketModel);
		_logger.LogDebug("<<< Exiting OnGet()");
    }

    public async Task<IActionResult> OnPost(CatalogItemViewModel productDetails)
    {
		_logger.LogDebug(">>> Entering OnPost(productDetails={ProductDetails})", productDetails);
        if (productDetails?.Id == null)
        {
            return RedirectToPage("/Index");
        }

		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var item = await _itemRepository.GetByIdAsync(productDetails.Id);
        if (item == null)
		_logger.LogDebug("OnPost(productDetails={ProductDetails}): item → {Item}", productDetails, item);
		_logger.LogInformation("OnPost(productDetails)={ProductDetails}: find query executed in {Arg1} ms", productDetails, __logStart.ElapsedMilliseconds);
        {
            return RedirectToPage("/Index");
        }

        var username = GetOrSetBasketCookieAndUserName();
		_logger.LogDebug("OnPost(productDetails={ProductDetails}): username → {Username}", productDetails, username);
        var basket = await _basketService.AddItemToBasket(username,
            productDetails.Id, item.Price);

        BasketModel = await _basketViewModelService.Map(basket);

		_logger.LogDebug("OnPost(productDetails={ProductDetails}): BasketModel → {BasketModel}", productDetails, BasketModel);
		_logger.LogDebug("<<< Exiting OnPost(productDetails={ProductDetails})", productDetails);
        return RedirectToPage();
    }

    public async Task OnPostUpdate(IEnumerable<BasketItemViewModel> items)
    {
		_logger.LogDebug(">>> Entering OnPostUpdate(items={Items})", items);
        if (!ModelState.IsValid)
        {
            return;
        }

        var basketView = await _basketViewModelService.GetOrCreateBasketForUser(GetOrSetBasketCookieAndUserName());
		_logger.LogDebug("OnPostUpdate(items={Items}): basketView → {BasketView}", items, basketView);
		_logger.LogDebug("OnPostUpdate(items={Items}): BasketModel → {BasketModel}", items, BasketModel);
        var updateModel = items.ToDictionary(b => b.Id.ToString(), b => b.Quantity);
		_logger.LogDebug("OnPostUpdate(items={Items}): updateModel → {UpdateModel}", items, updateModel);
        var basket = await _basketService.SetQuantities(basketView.Id, updateModel);
		_logger.LogDebug("OnPostUpdate(items={Items}): basket → {Basket}", items, basket);
        BasketModel = await _basketViewModelService.Map(basket);
		_logger.LogDebug("<<< Exiting OnPostUpdate(items={Items})", items);
    }

    private string GetOrSetBasketCookieAndUserName()
    {
		_logger.LogDebug(">>> Entering GetOrSetBasketCookieAndUserName()");
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

			_logger.LogDebug("GetOrSetBasketCookieAndUserName(): userName → {UserName}", userName);
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

		_logger.LogDebug("<<< Exiting GetOrSetBasketCookieAndUserName()");
        return userName;
    }
}
