using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Pages.Admin;

[Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS)]
public class EditCatalogItemModel : PageModel
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly ICatalogItemViewModelService _catalogItemViewModelService;

    public EditCatalogItemModel(ICatalogItemViewModelService catalogItemViewModelService)
    {
		_logger.LogDebug(">>> Entering EditCatalogItemModel(catalogItemViewModelService={CatalogItemViewModelService})", catalogItemViewModelService);
        _catalogItemViewModelService = catalogItemViewModelService;
		_logger.LogDebug("EditCatalogItemModel(catalogItemViewModelService={CatalogItemViewModelService}): _catalogItemViewModelService → {_catalogItemViewModelService}", catalogItemViewModelService, _catalogItemViewModelService);
    }

    [BindProperty]
    public CatalogItemViewModel CatalogModel { get; set; } = new CatalogItemViewModel();

    public void OnGet(CatalogItemViewModel catalogModel)
    {
		_logger.LogDebug(">>> Entering OnGet(catalogModel={CatalogModel})", catalogModel);
        CatalogModel = catalogModel;
		_logger.LogDebug("OnGet(catalogModel={CatalogModel}): CatalogModel → {CatalogModel1}", catalogModel, CatalogModel);
    }

    public async Task<IActionResult> OnPostAsync()
    {
		_logger.LogDebug(">>> Entering OnPostAsync()");
        if (ModelState.IsValid)
        {
            await _catalogItemViewModelService.UpdateCatalogItem(CatalogModel);
        }

		_logger.LogDebug("<<< Exiting OnPostAsync()");
        return RedirectToPage("/Admin/Index");
    }
}
