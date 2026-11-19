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

        return await _basketService.CountTotalBasketItems(anonymousId);
    }

    private string? GetAnnonymousIdFromCookie()
    {
		_logger.LogInformation("GetAnnonymousIdFromCookie(): GDPR-AUDIT [Art.30]: Personal data processing activity completed");
        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
        {
			_logger.LogInformation("GetAnnonymousIdFromCookie(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail");
            var id = Request.Cookies[Constants.BASKET_COOKIENAME];

            if (Guid.TryParse(id, out var _))
            {
				_logger.LogInformation("GetAnnonymousIdFromCookie(): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail");
				_logger.LogInformation("GetAnnonymousIdFromCookie(): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail");
				_logger.LogWarning("GetAnnonymousIdFromCookie(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail");
				_logger.LogWarning("GetAnnonymousIdFromCookie(): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail");
                return id;
            }
        }
        return null;
    }
}
