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
		_logger.LogInformation("ENTERING: OnAfterRenderAsync() method");
        if (firstRender)
        {
            catalogItems = await CatalogItemService.List();
            catalogTypes = await CatalogTypeService.List();
            catalogBrands = await CatalogBrandService.List();

            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
		_logger.LogWarning("OnAfterRenderAsync(firstRender)={FirstRender}): GDPR-AUDIT: Outbound notification recorded for the audit trail", firstRender);
    }

    private async void DetailsClick(int id)
    {
		_logger.LogInformation("ENTERING: DetailsClick() method");
        await DetailsComponent.Open(id);
    }

    private async Task CreateClick()
    {
		_logger.LogInformation("ENTERING: CreateClick() method");
        await CreateComponent.Open();
    }

    private async Task EditClick(int id)
    {
		_logger.LogInformation("ENTERING: EditClick() method");
        await EditComponent.Open(id);
    }

    private async Task DeleteClick(int id)
    {
		_logger.LogInformation("ENTERING: DeleteClick() method");
        await DeleteComponent.Open(id);
    }

    private async Task ReloadCatalogItems()
    {
		_logger.LogInformation("ENTERING: ReloadCatalogItems() method");
        catalogItems = await CatalogItemService.List();
        StateHasChanged();
    }
}
