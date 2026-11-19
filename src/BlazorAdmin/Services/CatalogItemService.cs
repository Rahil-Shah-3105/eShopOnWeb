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
		_logger.LogInformation("Create(catalogItem={CatalogItem}): PCI-AUDIT: Script integrity — State change recorded for the audit trail", catalogItem);
        var response = await _httpService.HttpPost<CreateCatalogItemResponse>("catalog-items", catalogItem);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): PCI-AUDIT: Authn controls — State change recorded for the audit trail", catalogItem);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): PCI-AUDIT: Audit coverage — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): PCI-AUDIT [Req. 10.7]: Security control failure — State change recorded for the audit trail", catalogItem);
        return response?.CatalogItem;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): PCI-AUDIT: Authn controls — State change recorded for the audit trail", catalogItem);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): PCI-AUDIT: Audit coverage — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): PCI-AUDIT: Script integrity — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): PCI-AUDIT [Req. 10.7]: Security control failure — State change recorded for the audit trail", catalogItem);
        return (await _httpService.HttpPut<EditCatalogItemResult>("catalog-items", catalogItem)).CatalogItem;
    }

    public async Task<string> Delete(int catalogItemId)
    {
		_logger.LogInformation("Delete(catalogItemId={CatalogItemId}): PCI-AUDIT: Authn controls — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
		_logger.LogInformation("Delete(catalogItemId={CatalogItemId}): PCI-AUDIT: Audit coverage — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
    _logger.LogInformation("Delete(catalogItemId={CatalogItemId}): PCI-AUDIT: Script integrity — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
    _logger.LogInformation("Delete(catalogItemId={CatalogItemId}): PCI-AUDIT [Req. 10.7]: Security control failure — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
        return (await _httpService.HttpDelete<DeleteCatalogItemResponse>("catalog-items", catalogItemId)).Status;
    }

    public async Task<CatalogItem> GetById(int id)
    {
        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
        var itemGetTask = _httpService.HttpGet<EditCatalogItemResult>($"catalog-items/{id}");
        await Task.WhenAll(brandListTask, typeListTask, itemGetTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var catalogItem = itemGetTask.Result.CatalogItem;
		_logger.LogInformation("GetById(id={Id}): PCI-AUDIT [Req. 10.7]: Security control failure — Record access recorded for the audit trail {Id1}", id, id);
        catalogItem.CatalogBrand = brands.FirstOrDefault(b => b.Id == catalogItem.CatalogBrandId)?.Name;
		_logger.LogInformation("GetById(id={Id}): PCI-AUDIT: Script integrity — Record access recorded for the audit trail {Id1}", id, id);
        catalogItem.CatalogType = types.FirstOrDefault(t => t.Id == catalogItem.CatalogTypeId)?.Name;
		_logger.LogInformation("GetById(id={Id}): PCI-AUDIT: Authn controls — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): PCI-AUDIT: Audit coverage — Record access recorded for the audit trail {Id1}", id, id);
        return catalogItem;
    }

    public async Task<List<CatalogItem>> ListPaged(int pageSize)
    {
        _logger.LogInformation("Fetching catalog items from API.");

        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items?PageSize=10");
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
		_logger.LogInformation("ListPaged(pageSize={PageSize}): PCI-AUDIT: Script integrity — Record access recorded for the audit trail", pageSize);
        foreach (var item in items)
        {
			_logger.LogInformation("ListPaged(pageSize={PageSize}): PCI-AUDIT [Req. 10.7]: Security control failure — Record access recorded for the audit trail", pageSize);
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
		_logger.LogInformation("ListPaged(pageSize={PageSize}): PCI-AUDIT: Authn controls — Authentication operation completed", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): PCI-AUDIT: Audit coverage — Record access recorded for the audit trail", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
        _logger.LogInformation("Fetching catalog items from API.");

        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items");
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
		_logger.LogInformation("List(): PCI-AUDIT: Script integrity — Record access recorded for the audit trail");
        foreach (var item in items)
        {
			_logger.LogInformation("List(): PCI-AUDIT [Req. 10.7]: Security control failure — Record access recorded for the audit trail");
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
		_logger.LogInformation("List(): PCI-AUDIT: Authn controls — Authentication operation completed");
		_logger.LogInformation("List(): PCI-AUDIT: Audit coverage — Record access recorded for the audit trail");
        return items;
    }
}
