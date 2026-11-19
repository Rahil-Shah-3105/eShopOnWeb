using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorAdmin.Helpers;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Pages.CatalogItemPage;

public partial class List : BlazorComponent
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    [Microsoft.AspNetCore.Components.Inject]
    public ICatalogItemService CatalogItemService { get; set; }

    [Microsoft.AspNetCore.Components.Inject]
    public ICatalogLookupDataService<CatalogBrand> CatalogBrandService { get; set; }

    [Microsoft.AspNetCore.Components.Inject]
    public ICatalogLookupDataService<CatalogType> CatalogTypeService { get; set; }

    private List<CatalogItem> catalogItems = new List<CatalogItem>();
    private List<CatalogType> catalogTypes = new List<CatalogType>();
    private List<CatalogBrand> catalogBrands = new List<CatalogBrand>();

    private Edit EditComponent { get; set; }
    private Delete DeleteComponent { get; set; }
    private Details DetailsComponent { get; set; }
    private Create CreateComponent { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            catalogItems = await CatalogItemService.List();
            catalogTypes = await CatalogTypeService.List();
            catalogBrands = await CatalogBrandService.List();

            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async void DetailsClick(int id)
    {
        await DetailsComponent.Open(id);
    }

    private async Task CreateClick()
    {
		_logger.LogInformation("CreateClick(): SOX-AUDIT: Computer operations — State change recorded for the audit trail");
		_logger.LogInformation("CreateClick(): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail");
    _logger.LogInformation("CreateClick(): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail");
    _logger.LogInformation("CreateClick(): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail");
        await CreateComponent.Open();
    }

    private async Task EditClick(int id)
    {
		_logger.LogInformation("EditClick(id={Id}): SOX-AUDIT: Computer operations — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("EditClick(id={Id}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("EditClick(id={Id}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("EditClick(id={Id}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail {Id1}", id, id);
        await EditComponent.Open(id);
    }

    private async Task DeleteClick(int id)
    {
		_logger.LogInformation("DeleteClick(id={Id}): SOX-AUDIT: Computer operations — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("DeleteClick(id={Id}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("DeleteClick(id={Id}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("DeleteClick(id={Id}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail {Id1}", id, id);
        await DeleteComponent.Open(id);
    }

    private async Task ReloadCatalogItems()
    {
		_logger.LogInformation("ReloadCatalogItems(): SOX-AUDIT: Change management (PCAOB AS 2201) — Record access recorded for the audit trail");
		_logger.LogInformation("ReloadCatalogItems(): SOX-AUDIT: Computer operations — Record access recorded for the audit trail");
		_logger.LogInformation("ReloadCatalogItems(): SOX-AUDIT: Audit log tamper — Record access recorded for the audit trail");
    _logger.LogInformation("ReloadCatalogItems(): SOX-AUDIT: Privileged access — Record access recorded for the audit trail");
    _logger.LogInformation("ReloadCatalogItems(): SOX-AUDIT: Access to programs and data — Record access recorded for the audit trail");
        catalogItems = await CatalogItemService.List();
        StateHasChanged();
    }
}
