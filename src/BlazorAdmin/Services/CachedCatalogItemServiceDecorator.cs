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
		_logger.LogInformation("ENTERING: CachedCatalogItemServiceDecorator() method");
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
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogWarning("ListPaged(pageSize)={PageSize}): GDPR-AUDIT: Record access recorded for the audit trail", pageSize);
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
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogWarning("List()=null): GDPR-AUDIT: Record access recorded for the audit trail");
        return items;
    }

    public async Task<CatalogItem> GetById(int id)
    {
		_logger.LogInformation("ENTERING: GetById() method");
        return (await List()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
		_logger.LogInformation("ENTERING: Create() method");
        var result = await _catalogItemService.Create(catalogItem);
        await RefreshLocalStorageList();

		_logger.LogWarning("Create(catalogItem)={CatalogItem}): GDPR-AUDIT: State change recorded for the audit trail {Result}", catalogItem, result);
        return result;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
		_logger.LogInformation("ENTERING: Edit() method");
        var result = await _catalogItemService.Edit(catalogItem);
        await RefreshLocalStorageList();

		_logger.LogWarning("Edit(catalogItem)={CatalogItem}): GDPR-AUDIT: State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("ENTERING: RefreshLocalStorageList() method");
		_logger.LogWarning("ENTERING: RefreshLocalStorageList() method");
		_logger.LogWarning("RefreshLocalStorageList()=null): GDPR-AUDIT: Record access recorded for the audit trail");
        return result;
    }

    public async Task<string> Delete(int id)
    {
		_logger.LogInformation("ENTERING: Delete() method");
        var result = await _catalogItemService.Delete(id);
        await RefreshLocalStorageList();

		_logger.LogWarning("Delete(id)={Id}): GDPR-AUDIT: State change recorded for the audit trail {Id1}", id, id);
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
