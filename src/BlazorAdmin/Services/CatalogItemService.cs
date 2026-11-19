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
		_logger.LogInformation("Create(catalogItem={CatalogItem}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", catalogItem);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", catalogItem);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", catalogItem);
        var response = await _httpService.HttpPost<CreateCatalogItemResponse>("catalog-items", catalogItem);
        return response?.CatalogItem;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", catalogItem);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", catalogItem);
        return (await _httpService.HttpPut<EditCatalogItemResult>("catalog-items", catalogItem)).CatalogItem;
    }

    public async Task<string> Delete(int catalogItemId)
    {
		_logger.LogInformation("Delete(catalogItemId={CatalogItemId}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
		_logger.LogInformation("Delete(catalogItemId={CatalogItemId}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
    _logger.LogInformation("Delete(catalogItemId={CatalogItemId}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
        return (await _httpService.HttpDelete<DeleteCatalogItemResponse>("catalog-items", catalogItemId)).Status;
    }

    public async Task<CatalogItem> GetById(int id)
    {
        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
        var itemGetTask = _httpService.HttpGet<EditCatalogItemResult>($"catalog-items/{id}");
		_logger.LogInformation("GetById(id={Id}): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): SOC2-AUDIT [CC6.2]: Authentication — Record access recorded for the audit trail {Id1}", id, id);
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
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC7.2]: Monitoring coverage — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail", pageSize);
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items?PageSize=10");
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
        foreach (var item in items)
        {
			_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail", pageSize);
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC6.1]: Authentication operation completed", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
        _logger.LogInformation("Fetching catalog items from API.");

        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
		_logger.LogInformation("List(): SOC2-AUDIT [CC7.2]: Monitoring coverage — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail");
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items");
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
        foreach (var item in items)
        {
			_logger.LogInformation("List(): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail");
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
		_logger.LogInformation("List(): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail");
		_logger.LogInformation("List(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
        return items;
    }
}
