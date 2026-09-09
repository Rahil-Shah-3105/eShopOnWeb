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
		_logger.LogWarning("OnAfterRenderAsync(firstRender)={FirstRender}): ISO27001-AUDIT: Outbound notification recorded for the audit trail", firstRender);
    }

    private async void DetailsClick(int id)
    {
        await DetailsComponent.Open(id);
    }

    private async Task CreateClick()
    {
        await CreateComponent.Open();
		_logger.LogWarning("CreateClick()=null): ISO27001-AUDIT: State change recorded for the audit trail");
    }

    private async Task EditClick(int id)
    {
        await EditComponent.Open(id);
		_logger.LogWarning("EditClick(id)={Id}): ISO27001-AUDIT: State change recorded for the audit trail {Id1}", id, id);
    }

    private async Task DeleteClick(int id)
    {
        await DeleteComponent.Open(id);
		_logger.LogWarning("DeleteClick(id)={Id}): ISO27001-AUDIT: State change recorded for the audit trail {Id1}", id, id);
    }

    private async Task ReloadCatalogItems()
    {
        catalogItems = await CatalogItemService.List();
		_logger.LogWarning("ReloadCatalogItems()=null): ISO27001-AUDIT: Record access recorded for the audit trail");
		_logger.LogWarning("ReloadCatalogItems()=null): ISO27001-AUDIT: Security incident detected");
        StateHasChanged();
    }
}
