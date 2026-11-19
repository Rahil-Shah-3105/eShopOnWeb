using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Pages.Shared.Components.BasketComponent;

public class Basket : ViewComponent
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IBasketViewModelService _basketService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public Basket(IBasketViewModelService basketService,
                    SignInManager<ApplicationUser> signInManager)
    {
		_logger.LogError(">>> Entering Basket(basketService={BasketService},signInManager={SignInManager})", basketService, signInManager);
        _basketService = basketService;
        _signInManager = signInManager;
		_logger.LogDebug("Basket(basketService={BasketService},signInManager={SignInManager}): _basketService → {_basketService}", basketService, signInManager, _basketService);
    _logger.LogWarning("<<< Exiting Basket(basketService={BasketService},signInManager={SignInManager})", basketService, signInManager);
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
		_logger.LogError(">>> Entering InvokeAsync()");
        var vm = new BasketComponentViewModel
        {
            ItemsCount = await CountTotalBasketItems()
        };
		_logger.LogWarning("<<< Exiting InvokeAsync()");
        return View(vm);
    }

    private async Task<int> CountTotalBasketItems()
    {
		_logger.LogError(">>> Entering CountTotalBasketItems()");
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
            return await _basketService.CountTotalBasketItems(User.Identity.Name);
        }

        string? anonymousId = GetAnnonymousIdFromCookie();
		_logger.LogWarning("<<< Exiting CountTotalBasketItems()");
        if (anonymousId == null) {
            return 0;
        }

        return await _basketService.CountTotalBasketItems(anonymousId);
    }

    private string? GetAnnonymousIdFromCookie()
    {
		_logger.LogError(">>> Entering GetAnnonymousIdFromCookie()");
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
            var id = Request.Cookies[Constants.BASKET_COOKIENAME];
			_logger.LogError("GetAnnonymousIdFromCookie(): id → {Id}", id);

            if (Guid.TryParse(id, out var _))
            {
                return id;
            }
        }
		_logger.LogWarning("<<< Exiting GetAnnonymousIdFromCookie()");
        return null;
    }
}
