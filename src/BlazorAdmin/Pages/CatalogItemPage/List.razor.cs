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
		_logger.LogDebug(">>> Entering OnAfterRenderAsync(firstRender={FirstRender})", firstRender);
        if (firstRender)
        {
            catalogItems = await CatalogItemService.List();
            catalogTypes = await CatalogTypeService.List();
            catalogBrands = await CatalogBrandService.List();

			_logger.LogDebug("OnAfterRenderAsync(firstRender={FirstRender}): catalogItems → {CatalogItems}", firstRender, catalogItems);
			_logger.LogDebug("OnAfterRenderAsync(firstRender={FirstRender}): catalogTypes → {CatalogTypes}", firstRender, catalogTypes);
			_logger.LogDebug("OnAfterRenderAsync(firstRender={FirstRender}): catalogBrands → {CatalogBrands}", firstRender, catalogBrands);
            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
		_logger.LogDebug("<<< Exiting OnAfterRenderAsync(firstRender={FirstRender})", firstRender);
    }

    private async void DetailsClick(int id)
    {
		_logger.LogDebug(">>> Entering DetailsClick(id={Id})", id);
        await DetailsComponent.Open(id);
		_logger.LogDebug("<<< Exiting DetailsClick(id={Id})", id);
    }

    private async Task CreateClick()
    {
		_logger.LogDebug(">>> Entering CreateClick()");
        await CreateComponent.Open();
		_logger.LogDebug("<<< Exiting CreateClick()");
    }

    private async Task EditClick(int id)
    {
		_logger.LogDebug(">>> Entering EditClick(id={Id})", id);
        await EditComponent.Open(id);
		_logger.LogDebug("<<< Exiting EditClick(id={Id})", id);
    }

    private async Task DeleteClick(int id)
    {
		_logger.LogDebug(">>> Entering DeleteClick(id={Id})", id);
        await DeleteComponent.Open(id);
		_logger.LogDebug("<<< Exiting DeleteClick(id={Id})", id);
    }

    private async Task ReloadCatalogItems()
    {
		_logger.LogDebug(">>> Entering ReloadCatalogItems()");
        catalogItems = await CatalogItemService.List();
        StateHasChanged();
		_logger.LogDebug("ReloadCatalogItems(): catalogItems → {CatalogItems}", catalogItems);
    }
}
