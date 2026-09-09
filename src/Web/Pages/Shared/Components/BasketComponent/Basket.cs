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
        _basketService = basketService;
		_logger.LogInformation("Basket(basketService,signInManager)={BasketService},{SignInManager}: M2614-AUDIT: Authentication operation completed", basketService, signInManager);
        _signInManager = signInManager;
		_logger.LogInformation("Basket(basketService,signInManager)={BasketService},{SignInManager}: SOC2-AUDIT [CC6.1]: Authentication operation completed", basketService, signInManager);
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var vm = new BasketComponentViewModel
        {
            ItemsCount = await CountTotalBasketItems()
        };
		_logger.LogWarning("InvokeAsync()=null): M2614-AUDIT: Outbound notification recorded for the audit trail");
        return View(vm);
    }

    private async Task<int> CountTotalBasketItems()
    {
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            Guard.Against.Null(User?.Identity?.Name, nameof(User.Identity.Name));
            return await _basketService.CountTotalBasketItems(User.Identity.Name);
        }

        string? anonymousId = GetAnnonymousIdFromCookie();
        if (anonymousId == null)
            return 0;

		_logger.LogInformation("CountTotalBasketItems()=null): M2614-AUDIT: Authentication operation completed");
		_logger.LogInformation("CountTotalBasketItems()=null): SOC2-AUDIT [CC6.1]: Authentication operation completed");
        return await _basketService.CountTotalBasketItems(anonymousId);
    }

    private string? GetAnnonymousIdFromCookie()
    {
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
            var id = Request.Cookies[Constants.BASKET_COOKIENAME];

            if (Guid.TryParse(id, out var _))
            {
                return id;
            }
        }
		_logger.LogWarning("GetAnnonymousIdFromCookie()=null): M2614-AUDIT: Record access recorded for the audit trail");
        return null;
    }
}
