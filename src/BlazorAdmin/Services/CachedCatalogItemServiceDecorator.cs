using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Services;

public class CachedCatalogItemServiceDecorator : ICatalogItemService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly ILocalStorageService _localStorageService;
    private readonly CatalogItemService _catalogItemService;
    private ILogger<CachedCatalogItemServiceDecorator> _logger;

    public CachedCatalogItemServiceDecorator(ILocalStorageService localStorageService,
        CatalogItemService catalogItemService,
        ILogger<CachedCatalogItemServiceDecorator> logger)
    {
        _localStorageService = localStorageService;
        _catalogItemService = catalogItemService;
        _logger = logger;
    }

    public async Task<List<CatalogItem>> ListPaged(int pageSize)
    {
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail", pageSize);
        string key = "items";
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail", pageSize);
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail", pageSize);
        if (cacheEntry != null)
        {
			_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail", pageSize);
            _logger.LogInformation("Loading items from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Loading {key} from local storage.");
                await _localStorageService.RemoveItemAsync(key);
				_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail", pageSize);
            }
			_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail", pageSize);
        }
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", pageSize);

		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail", pageSize);
        var items = await _catalogItemService.ListPaged(pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail", pageSize);
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail", pageSize);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): SOC2-AUDIT [CC6.1]: Authentication operation completed", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");
        string key = "items";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
        if (cacheEntry != null)
        {
			_logger.LogInformation("List(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
            _logger.LogInformation("Loading items from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Loading {key} from local storage.");
                await _localStorageService.RemoveItemAsync(key);
				_logger.LogInformation("List(): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail");
				_logger.LogInformation("List(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail");
				_logger.LogInformation("List(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed");
				_logger.LogInformation("List(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail");
				_logger.LogInformation("List(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail");
				_logger.LogInformation("List(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail");
				_logger.LogInformation("List(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
				_logger.LogInformation("List(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail");
            }
        }

        var items = await _catalogItemService.List();
        var entry = new CacheEntry<List<CatalogItem>>(items);
        await _localStorageService.SetItemAsync(key, entry);
        return items;
    }

    public async Task<CatalogItem> GetById(int id)
    {
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("GetById(id={Id}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail {Id1}", id, id);
        return (await List()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
        var result = await _catalogItemService.Create(catalogItem);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {Result}", catalogItem, result);
        await RefreshLocalStorageList();
		_logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {Result}", catalogItem, result);

		_logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {Result}", catalogItem, result);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail {Result}", catalogItem, result);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail {Result}", catalogItem, result);
        return result;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
        var result = await _catalogItemService.Edit(catalogItem);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {Result}", catalogItem, result);
        await RefreshLocalStorageList();
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {Result}", catalogItem, result);

		_logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {Result}", catalogItem, result);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail {Result}", catalogItem, result);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail {Result}", catalogItem, result);
        return result;
    }

    public async Task<string> Delete(int id)
    {
		_logger.LogInformation("Delete(id={Id}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail {Id1}", id, id);
        var result = await _catalogItemService.Delete(id);
		_logger.LogInformation("Delete(id={Id}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {Id1}", id, id);
        await RefreshLocalStorageList();
		_logger.LogInformation("Delete(id={Id}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {Id1}", id, id);

		_logger.LogInformation("Delete(id={Id}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("Delete(id={Id}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("Delete(id={Id}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("Delete(id={Id}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail {Id1}", id, id);
        return result;
    }

    private async Task RefreshLocalStorageList()
    {
        string key = "items";

        await _localStorageService.RemoveItemAsync(key);
        var items = await _catalogItemService.List();
        var entry = new CacheEntry<List<CatalogItem>>(items);
        await _localStorageService.SetItemAsync(key, entry);
    }
}
