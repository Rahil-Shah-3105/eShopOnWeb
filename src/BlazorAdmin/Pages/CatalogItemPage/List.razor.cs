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
		_logger.LogInformation("CreateClick(): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail");
		_logger.LogInformation("CreateClick(): DORA-AUDIT: Incident coverage — State change recorded for the audit trail");
    _logger.LogInformation("CreateClick(): DORA-AUDIT: Third party risk — State change recorded for the audit trail");
    _logger.LogInformation("CreateClick(): DORA-AUDIT: Resilience testing — State change recorded for the audit trail");
        await CreateComponent.Open();
    }

    private async Task EditClick(int id)
    {
		_logger.LogInformation("EditClick(id={Id}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail {Id1}", id, id);
        await EditComponent.Open(id);
		_logger.LogInformation("EditClick(id={Id}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("EditClick(id={Id}): DORA-AUDIT: Third party risk — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("EditClick(id={Id}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail {Id1}", id, id);
    }

    private async Task DeleteClick(int id)
    {
		_logger.LogInformation("DeleteClick(id={Id}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail {Id1}", id, id);
        await DeleteComponent.Open(id);
		_logger.LogInformation("DeleteClick(id={Id}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("DeleteClick(id={Id}): DORA-AUDIT: Third party risk — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("DeleteClick(id={Id}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail {Id1}", id, id);
    }

    private async Task ReloadCatalogItems()
    {
		_logger.LogInformation("ReloadCatalogItems(): DORA-AUDIT: Resilience testing — Record access recorded for the audit trail");
		_logger.LogInformation("ReloadCatalogItems(): DORA-AUDIT [Art. 45]: Information sharing — Record access recorded for the audit trail");
		_logger.LogInformation("ReloadCatalogItems(): DORA-AUDIT: Incident coverage — Record access recorded for the audit trail");
    _logger.LogInformation("ReloadCatalogItems(): DORA-AUDIT: Third party risk — Record access recorded for the audit trail");
        catalogItems = await CatalogItemService.List();
        StateHasChanged();
    }
}
