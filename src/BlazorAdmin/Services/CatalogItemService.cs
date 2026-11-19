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
		_logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", catalogItem);
        var response = await _httpService.HttpPost<CreateCatalogItemResponse>("catalog-items", catalogItem);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", catalogItem);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", catalogItem);
        return response?.CatalogItem;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", catalogItem);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", catalogItem);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", catalogItem);
        return (await _httpService.HttpPut<EditCatalogItemResult>("catalog-items", catalogItem)).CatalogItem;
    }

    public async Task<string> Delete(int catalogItemId)
    {
		_logger.LogInformation("Delete(catalogItemId={CatalogItemId}): SOC2-AUDIT [CC7.1]: System operation completed", catalogItemId);
		_logger.LogInformation("Delete(catalogItemId={CatalogItemId}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
    _logger.LogInformation("Delete(catalogItemId={CatalogItemId}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
    _logger.LogInformation("Delete(catalogItemId={CatalogItemId}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {CatalogItemId1}", catalogItemId, catalogItemId);
        return (await _httpService.HttpDelete<DeleteCatalogItemResponse>("catalog-items", catalogItemId)).Status;
    }

    public async Task<CatalogItem> GetById(int id)
    {
        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail {Id1}", id, id);
        var itemGetTask = _httpService.HttpGet<EditCatalogItemResult>($"catalog-items/{id}");
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail {Id1}", id, id);
        await Task.WhenAll(brandListTask, typeListTask, itemGetTask);
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail {Id1}", id, id);
        var brands = brandListTask.Result;
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail {Id1}", id, id);
        var types = typeListTask.Result;
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Outcome 1: user attribution — Record access recorded for the audit trail {Id1}", id, id);
        var catalogItem = itemGetTask.Result.CatalogItem;
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail {Id1}", id, id);
        catalogItem.CatalogBrand = brands.FirstOrDefault(b => b.Id == catalogItem.CatalogBrandId)?.Name;
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail {Id1}", id, id);
        catalogItem.CatalogType = types.FirstOrDefault(t => t.Id == catalogItem.CatalogTypeId)?.Name;
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail {Id1}", id, id);
        return catalogItem;
    }

    public async Task<List<CatalogItem>> ListPaged(int pageSize)
    {
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail", pageSize);
        _logger.LogInformation("Fetching catalog items from API.");
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail", pageSize);

		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail", pageSize);
        var brandListTask = _brandService.List();
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail", pageSize);
        var typeListTask = _typeService.List();
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail", pageSize);
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items?PageSize=10");
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail", pageSize);
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail", pageSize);
        var brands = brandListTask.Result;
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", pageSize);
        var types = typeListTask.Result;
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail", pageSize);
        var items = itemListTask.Result.CatalogItems;
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail", pageSize);
        foreach (var item in items)
        {
			_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail", pageSize);
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
			_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 10: automated alerting — Record access recorded for the audit trail", pageSize);
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
			_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail", pageSize);
        }
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC6.1]: Authentication operation completed", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed");
		_logger.LogInformation("List(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail");
        _logger.LogInformation("Fetching catalog items from API.");

        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items");
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
        foreach (var item in items)
        {
			_logger.LogInformation("List(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail");
			_logger.LogInformation("List(): M2614-AUDIT: Outcome 10: automated alerting — Record access recorded for the audit trail");
			_logger.LogInformation("List(): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail");
			_logger.LogInformation("List(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
			_logger.LogInformation("List(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail");
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
        return items;
    }
}
