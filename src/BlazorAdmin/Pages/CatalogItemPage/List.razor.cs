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
		_logger.LogWarning("CreateClick(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail");
		_logger.LogWarning("CreateClick(): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail");
    _logger.LogInformation("CreateClick(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail");
    _logger.LogInformation("CreateClick(): GDPR-AUDIT: Breach notification — State change recorded for the audit trail");
        await CreateComponent.Open();
    }

    private async Task EditClick(int id)
    {
		_logger.LogWarning("EditClick(id={Id}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogWarning("EditClick(id={Id}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("EditClick(id={Id}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("EditClick(id={Id}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {Id1}", id, id);
        await EditComponent.Open(id);
    }

    private async Task DeleteClick(int id)
    {
		_logger.LogWarning("DeleteClick(id={Id}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogWarning("DeleteClick(id={Id}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("DeleteClick(id={Id}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("DeleteClick(id={Id}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {Id1}", id, id);
        await DeleteComponent.Open(id);
    }

    private async Task ReloadCatalogItems()
    {
		_logger.LogInformation("ReloadCatalogItems(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail");
		_logger.LogWarning("ReloadCatalogItems(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail");
		_logger.LogWarning("ReloadCatalogItems(): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail");
    _logger.LogInformation("ReloadCatalogItems(): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail");
    _logger.LogInformation("ReloadCatalogItems(): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail");
        catalogItems = await CatalogItemService.List();
        StateHasChanged();
    }
}
