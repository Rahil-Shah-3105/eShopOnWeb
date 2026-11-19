using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.Logging;


namespace BlazorAdmin.Services;

public class CatalogItemService : ICatalogItemService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly ICatalogLookupDataService<CatalogBrand> _brandService;
    private readonly ICatalogLookupDataService<CatalogType> _typeService;
    private readonly HttpService _httpService;
    private readonly ILogger<CatalogItemService> _logger;

    public CatalogItemService(ICatalogLookupDataService<CatalogBrand> brandService,
        ICatalogLookupDataService<CatalogType> typeService,
        HttpService httpService,
        ILogger<CatalogItemService> logger)
    {
        _brandService = brandService;
        _typeService = typeService;
        _httpService = httpService;
        _logger = logger;
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
		_logger.LogInformation("Create(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", catalogItem);
		_logger.LogWarning("Create(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", catalogItem);
		_logger.LogWarning("Create(catalogItem={CatalogItem}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", catalogItem);
        var response = await _httpService.HttpPost<CreateCatalogItemResponse>("catalog-items", catalogItem);
        return response?.CatalogItem;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
		_logger.LogWarning("Edit(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", catalogItem);
		_logger.LogWarning("Edit(catalogItem={CatalogItem}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", catalogItem);
        return (await _httpService.HttpPut<EditCatalogItemResult>("catalog-items", catalogItem)).CatalogItem;
    }

    public async Task<string> Delete(int catalogItemId)
    {
		_logger.LogWarning("Delete(catalogItemId={CatalogItemId}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
		_logger.LogWarning("Delete(catalogItemId={CatalogItemId}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
    _logger.LogInformation("Delete(catalogItemId={CatalogItemId}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
    _logger.LogInformation("Delete(catalogItemId={CatalogItemId}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
        return (await _httpService.HttpDelete<DeleteCatalogItemResponse>("catalog-items", catalogItemId)).Status;
    }

    public async Task<CatalogItem> GetById(int id)
    {
        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
        var itemGetTask = _httpService.HttpGet<EditCatalogItemResult>($"catalog-items/{id}");
		_logger.LogInformation("GetById(id={Id}): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogWarning("GetById(id={Id}): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): GDPR-AUDIT [Art.30]: Personal data processing activity completed", id);
		_logger.LogWarning("GetById(id={Id}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail {Id1}", id, id);
        await Task.WhenAll(brandListTask, typeListTask, itemGetTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var catalogItem = itemGetTask.Result.CatalogItem;
        catalogItem.CatalogBrand = brands.FirstOrDefault(b => b.Id == catalogItem.CatalogBrandId)?.Name;
        catalogItem.CatalogType = types.FirstOrDefault(t => t.Id == catalogItem.CatalogTypeId)?.Name;
        return catalogItem;
    }

    public async Task<List<CatalogItem>> ListPaged(int pageSize)
    {
        _logger.LogInformation("Fetching catalog items from API.");

        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT: Transfers to third countries — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail", pageSize);
		_logger.LogWarning("ListPaged(pageSize={PageSize}): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art.30]: Personal data processing activity completed", pageSize);
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items?PageSize=10");
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
        foreach (var item in items)
        {
			_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail", pageSize);
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
		_logger.LogWarning("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
        _logger.LogInformation("Fetching catalog items from API.");

        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
		_logger.LogInformation("List(): GDPR-AUDIT: Transfers to third countries — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail");
		_logger.LogWarning("List(): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art.30]: Personal data processing activity completed");
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items");
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
        foreach (var item in items)
        {
			_logger.LogInformation("List(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail");
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
		_logger.LogWarning("List(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail");
        return items;
    }
}
