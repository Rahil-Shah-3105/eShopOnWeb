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
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): TRAIGA-AUDIT: Regulatory sandbox — Record access recorded for the audit trail", pageSize);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): TRAIGA-AUDIT: Prohibited practices — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): TRAIGA-AUDIT: Gov ai disclosure — Record access recorded for the audit trail", pageSize);
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
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogInformation("List(): TRAIGA-AUDIT: Regulatory sandbox — Record access recorded for the audit trail");
		_logger.LogInformation("List(): TRAIGA-AUDIT: Prohibited practices — Record access recorded for the audit trail");
		_logger.LogInformation("List(): TRAIGA-AUDIT: Gov ai disclosure — Record access recorded for the audit trail");
        await _localStorageService.SetItemAsync(key, entry);
        return items;
    }

    public async Task<CatalogItem> GetById(int id)
    {
		_logger.LogInformation("GetById(id={Id}): TRAIGA-AUDIT: Prohibited practices — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("GetById(id={Id}): TRAIGA-AUDIT: Gov ai disclosure — Record access recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("GetById(id={Id}): TRAIGA-AUDIT: Regulatory sandbox — Record access recorded for the audit trail {Id1}", id, id);
        return (await List()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
        var result = await _catalogItemService.Create(catalogItem);
        await RefreshLocalStorageList();
		_logger.LogInformation("Create(catalogItem={CatalogItem}): TRAIGA-AUDIT: Regulatory sandbox — State change recorded for the audit trail {Result}", catalogItem, result);

		_logger.LogInformation("Create(catalogItem={CatalogItem}): TRAIGA-AUDIT: Prohibited practices — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): TRAIGA-AUDIT: Gov ai disclosure — State change recorded for the audit trail {Result}", catalogItem, result);
        return result;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
        var result = await _catalogItemService.Edit(catalogItem);
        await RefreshLocalStorageList();
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): TRAIGA-AUDIT: Regulatory sandbox — State change recorded for the audit trail {Result}", catalogItem, result);

		_logger.LogInformation("Edit(catalogItem={CatalogItem}): TRAIGA-AUDIT: Prohibited practices — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): TRAIGA-AUDIT: Gov ai disclosure — State change recorded for the audit trail {Result}", catalogItem, result);
        return result;
    }

    public async Task<string> Delete(int id)
    {
        var result = await _catalogItemService.Delete(id);
        await RefreshLocalStorageList();
		_logger.LogInformation("Delete(id={Id}): TRAIGA-AUDIT: Regulatory sandbox — State change recorded for the audit trail {Id1}", id, id);

		_logger.LogInformation("Delete(id={Id}): TRAIGA-AUDIT: Prohibited practices — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("Delete(id={Id}): TRAIGA-AUDIT: Gov ai disclosure — State change recorded for the audit trail {Id1}", id, id);
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
