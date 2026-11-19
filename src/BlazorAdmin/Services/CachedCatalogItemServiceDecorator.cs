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
        string key = "items";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
        if (cacheEntry != null)
        {
            _logger.LogInformation("Loading items from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Loading {key} from local storage.");
                await _localStorageService.RemoveItemAsync(key);
            }
        }

        var items = await _catalogItemService.ListPaged(pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — Record access recorded for the audit trail", pageSize);
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): NIS2-AUDIT: Security events — Record access recorded for the audit trail", pageSize);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): NIS2-AUDIT [Art. 20]: Governance — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — Record access recorded for the audit trail", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
        string key = "items";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
        if (cacheEntry != null)
        {
            _logger.LogInformation("Loading items from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Loading {key} from local storage.");
                await _localStorageService.RemoveItemAsync(key);
            }
        }

        var items = await _catalogItemService.List();
		_logger.LogInformation("List(): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — Record access recorded for the audit trail");
		_logger.LogInformation("List(): NIS2-AUDIT: Security events — Record access recorded for the audit trail");
		_logger.LogInformation("List(): NIS2-AUDIT [Art. 20]: Governance — Record access recorded for the audit trail");
		_logger.LogInformation("List(): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — Record access recorded for the audit trail");
        var entry = new CacheEntry<List<CatalogItem>>(items);
        await _localStorageService.SetItemAsync(key, entry);
        return items;
    }

    public async Task<CatalogItem> GetById(int id)
    {
		_logger.LogInformation("GetById(id={Id}): NIS2-AUDIT [Art. 20]: Governance — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): NIS2-AUDIT: Security events — Record access recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("GetById(id={Id}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — Record access recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("GetById(id={Id}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — Record access recorded for the audit trail {Id1}", id, id);
        return (await List()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
        var result = await _catalogItemService.Create(catalogItem);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail {Result}", catalogItem, result);
        await RefreshLocalStorageList();
		_logger.LogInformation("Create(catalogItem={CatalogItem}): NIS2-AUDIT: Security events — State change recorded for the audit trail {Result}", catalogItem, result);

		_logger.LogInformation("Create(catalogItem={CatalogItem}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail {Result}", catalogItem, result);
        return result;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
        var result = await _catalogItemService.Edit(catalogItem);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail {Result}", catalogItem, result);
        await RefreshLocalStorageList();
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): NIS2-AUDIT: Security events — State change recorded for the audit trail {Result}", catalogItem, result);

		_logger.LogInformation("Edit(catalogItem={CatalogItem}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail {Result}", catalogItem, result);
        return result;
    }

    public async Task<string> Delete(int id)
    {
        var result = await _catalogItemService.Delete(id);
		_logger.LogInformation("Delete(id={Id}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail {Id1}", id, id);
        await RefreshLocalStorageList();
		_logger.LogInformation("Delete(id={Id}): NIS2-AUDIT: Security events — State change recorded for the audit trail {Id1}", id, id);

		_logger.LogInformation("Delete(id={Id}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("Delete(id={Id}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail {Id1}", id, id);
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
