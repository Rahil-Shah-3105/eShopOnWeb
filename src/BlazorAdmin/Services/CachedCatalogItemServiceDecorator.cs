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
    private readonly ILocalStorageService _localStorageService;
    private readonly CatalogItemService _catalogItemService;
    private ILogger<CachedCatalogItemServiceDecorator> _logger;

    public CachedCatalogItemServiceDecorator(ILocalStorageService localStorageService,
        CatalogItemService catalogItemService,
        ILogger<CachedCatalogItemServiceDecorator> logger)
    {
        _localStorageService = localStorageService;
		_logger.LogDebug("CachedCatalogItemServiceDecorator(localStorageService={LocalStorageService},catalogItemService={CatalogItemService},logger={Logger}): _localStorageService → {_localStorageService}", localStorageService, catalogItemService, logger, _localStorageService);
        _catalogItemService = catalogItemService;
		_logger.LogDebug("CachedCatalogItemServiceDecorator(localStorageService={LocalStorageService},catalogItemService={CatalogItemService},logger={Logger}): _catalogItemService → {_catalogItemService}", localStorageService, catalogItemService, logger, _catalogItemService);
        _logger = logger;
		_logger.LogDebug(">>> Entering CachedCatalogItemServiceDecorator(localStorageService={LocalStorageService},catalogItemService={CatalogItemService},logger={Logger})", localStorageService, catalogItemService, logger);
    }

    public async Task<List<CatalogItem>> ListPaged(int pageSize)
    {
		_logger.LogDebug(">>> Entering ListPaged(pageSize={PageSize})", pageSize);
        string key = "items";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
		_logger.LogDebug("ListPaged(pageSize={PageSize}): cacheEntry → {CacheEntry}", pageSize, cacheEntry);
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
		_logger.LogDebug("ListPaged(pageSize={PageSize}): items → {Items}", pageSize, items);
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogDebug("ListPaged(pageSize={PageSize}): entry → {Entry}", pageSize, entry);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogDebug("<<< Exiting ListPaged(pageSize={PageSize})", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
		_logger.LogDebug(">>> Entering List()");
        string key = "items";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
		_logger.LogDebug("List(): cacheEntry → {CacheEntry}", cacheEntry);
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
		_logger.LogDebug("List(): items → {Items}", items);
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogDebug("List(): entry → {Entry}", entry);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogDebug("<<< Exiting List()");
        return items;
    }

    public async Task<CatalogItem> GetById(int id)
    {
		_logger.LogDebug(">>> Entering GetById(id={Id})", id);
		_logger.LogDebug("<<< Exiting GetById(id={Id})", id);
        return (await List()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
		_logger.LogDebug(">>> Entering Create(catalogItem={CatalogItem})", catalogItem);
        var result = await _catalogItemService.Create(catalogItem);
        await RefreshLocalStorageList();
		_logger.LogDebug("Create(catalogItem={CatalogItem}): result → {Result}", catalogItem, result);

		_logger.LogDebug("<<< Exiting Create(catalogItem={CatalogItem})", catalogItem);
        return result;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
		_logger.LogDebug(">>> Entering Edit(catalogItem={CatalogItem})", catalogItem);
        var result = await _catalogItemService.Edit(catalogItem);
        await RefreshLocalStorageList();
		_logger.LogDebug("Edit(catalogItem={CatalogItem}): result → {Result}", catalogItem, result);

		_logger.LogDebug("<<< Exiting Edit(catalogItem={CatalogItem})", catalogItem);
		_logger.LogDebug(">>> Entering RefreshLocalStorageList()");
		_logger.LogDebug("<<< Exiting RefreshLocalStorageList()");
        return result;
    }

    public async Task<string> Delete(int id)
    {
		_logger.LogDebug(">>> Entering Delete(id={Id})", id);
        var result = await _catalogItemService.Delete(id);
        await RefreshLocalStorageList();
		_logger.LogDebug("Delete(id={Id}): result → {Result}", id, result);

		_logger.LogDebug("<<< Exiting Delete(id={Id})", id);
        return result;
    }

    private async Task RefreshLocalStorageList()
    {
        string key = "items";

        await _localStorageService.RemoveItemAsync(key);
        var items = await _catalogItemService.List();
		_logger.LogDebug("RefreshLocalStorageList(): items → {Items}", items);
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogDebug("RefreshLocalStorageList(): entry → {Entry}", entry);
        await _localStorageService.SetItemAsync(key, entry);
    }
}
