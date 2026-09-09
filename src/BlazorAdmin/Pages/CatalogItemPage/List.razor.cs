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

		_logger.LogWarning("OnAfterRenderAsync(firstRender)={FirstRender}): M2614-AUDIT: Outbound notification recorded for the audit trail", firstRender);
        await base.OnAfterRenderAsync(firstRender);
    }

    private async void DetailsClick(int id)
    {
        await DetailsComponent.Open(id);
    }

    private async Task CreateClick()
    {
		_logger.LogWarning("CreateClick()=null): M2614-AUDIT: State change recorded for the audit trail");
        await CreateComponent.Open();
    }

    private async Task EditClick(int id)
    {
		_logger.LogWarning("EditClick(id)={Id}): M2614-AUDIT: State change recorded for the audit trail {Id1}", id, id);
        await EditComponent.Open(id);
    }

    private async Task DeleteClick(int id)
    {
		_logger.LogWarning("DeleteClick(id)={Id}): M2614-AUDIT: State change recorded for the audit trail {Id1}", id, id);
        await DeleteComponent.Open(id);
    }

    private async Task ReloadCatalogItems()
    {
		_logger.LogWarning("ReloadCatalogItems()=null): M2614-AUDIT: Security incident detected");
		_logger.LogWarning("ReloadCatalogItems()=null): M2614-AUDIT: Record access recorded for the audit trail");
        catalogItems = await CatalogItemService.List();
        StateHasChanged();
    }
}
