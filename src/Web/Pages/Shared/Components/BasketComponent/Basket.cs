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
        _signInManager = signInManager;
		_logger.LogInformation("Basket(basketService={BasketService}, signInManager={SignInManager}): SOC2-AUDIT [CC6.1]: Authentication operation completed", basketService, signInManager);
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var vm = new BasketComponentViewModel
        {
            ItemsCount = await CountTotalBasketItems()
        };
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

		_logger.LogInformation("CountTotalBasketItems(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
        return await _basketService.CountTotalBasketItems(anonymousId);
    }

    private string? GetAnnonymousIdFromCookie()
    {
		_logger.LogInformation("GetAnnonymousIdFromCookie(): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail");
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
			_logger.LogInformation("GetAnnonymousIdFromCookie(): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail");
            var id = Request.Cookies[Constants.BASKET_COOKIENAME];

            if (Guid.TryParse(id, out var _))
            {
				_logger.LogInformation("GetAnnonymousIdFromCookie(): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail");
				_logger.LogInformation("GetAnnonymousIdFromCookie(): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail");
				_logger.LogInformation("GetAnnonymousIdFromCookie(): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail");
				_logger.LogInformation("GetAnnonymousIdFromCookie(): SOC2-AUDIT [CC6.2]: Authentication — Record access recorded for the audit trail");
                return id;
            }
        }
        return null;
    }
}
