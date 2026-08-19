using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Pages;

public class IndexModel : PageModel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly ICatalogViewModelService _catalogViewModelService;

    public IndexModel(ICatalogViewModelService catalogViewModelService)
    {
		_logger.LogDebug(">>> Entering IndexModel(catalogViewModelService={CatalogViewModelService})", catalogViewModelService);
        _catalogViewModelService = catalogViewModelService;
		_logger.LogDebug("IndexModel(catalogViewModelService={CatalogViewModelService}): _catalogViewModelService → {_catalogViewModelService}", catalogViewModelService, _catalogViewModelService);
		_logger.LogDebug("<<< Exiting IndexModel(catalogViewModelService={CatalogViewModelService})", catalogViewModelService);
    }

    public required CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();

    public async Task OnGet(CatalogIndexViewModel catalogModel, int? pageId)
    {
		_logger.LogDebug(">>> Entering OnGet(catalogModel={CatalogModel},pageId={PageId})", catalogModel, pageId);
        CatalogModel = await _catalogViewModelService.GetCatalogItems(pageId ?? 0, Constants.ITEMS_PER_PAGE, catalogModel.BrandFilterApplied, catalogModel.TypesFilterApplied);
		_logger.LogDebug("OnGet(catalogModel={CatalogModel},pageId={PageId}): CatalogModel → {CatalogModel1}", catalogModel, pageId, CatalogModel);
		_logger.LogDebug("<<< Exiting OnGet(catalogModel={CatalogModel},pageId={PageId})", catalogModel, pageId);
    }
}
