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
		_logger.LogDebug("CachedCatalogItemServiceDecorator(localStorageService={LocalStorageService},catalogItemService={CatalogItemService},logger={Logger}): _localStorageService → {_localStorageService}", localStorageService, catalogItemService, logger, _localStorageService);
        _catalogItemService = catalogItemService;
		_logger.LogDebug("CachedCatalogItemServiceDecorator(localStorageService={LocalStorageService},catalogItemService={CatalogItemService},logger={Logger}): _catalogItemService → {_catalogItemService}", localStorageService, catalogItemService, logger, _catalogItemService);
        _logger = logger;
		_logger.LogError(">>> Entering CachedCatalogItemServiceDecorator(localStorageService={LocalStorageService},catalogItemService={CatalogItemService},logger={Logger})", localStorageService, catalogItemService, logger);
    _logger.LogWarning("<<< Exiting CachedCatalogItemServiceDecorator(localStorageService={LocalStorageService},catalogItemService={CatalogItemService},logger={Logger})", localStorageService, catalogItemService, logger);
    }

    public async Task<List<CatalogItem>> ListPaged(int pageSize)
    {
		_logger.LogError(">>> Entering ListPaged(pageSize={PageSize})", pageSize);
        string key = "items";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
		_logger.LogError("ListPaged(pageSize={PageSize}): cacheEntry → {CacheEntry}", pageSize, cacheEntry);
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
		_logger.LogError("ListPaged(pageSize={PageSize}): items → {Items}", pageSize, items);
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogError("ListPaged(pageSize={PageSize}): entry → {Entry}", pageSize, entry);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogWarning("<<< Exiting ListPaged(pageSize={PageSize})", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
		_logger.LogError(">>> Entering List()");
        string key = "items";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
		_logger.LogError("List(): cacheEntry → {CacheEntry}", cacheEntry);
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
		_logger.LogError("List(): items → {Items}", items);
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogError("List(): entry → {Entry}", entry);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogWarning("<<< Exiting List()");
        return items;
    }

    public async Task<CatalogItem> GetById(int id)
    {
		_logger.LogError(">>> Entering GetById(id={Id})", id);
		_logger.LogWarning("<<< Exiting GetById(id={Id})", id);
        return (await List()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
		_logger.LogError(">>> Entering Create(catalogItem={CatalogItem})", catalogItem);
        var result = await _catalogItemService.Create(catalogItem);
        await RefreshLocalStorageList();
		_logger.LogError("Create(catalogItem={CatalogItem}): result → {Result}", catalogItem, result);

		_logger.LogWarning("<<< Exiting Create(catalogItem={CatalogItem})", catalogItem);
        return result;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
		_logger.LogError(">>> Entering Edit(catalogItem={CatalogItem})", catalogItem);
        var result = await _catalogItemService.Edit(catalogItem);
        await RefreshLocalStorageList();
		_logger.LogError("Edit(catalogItem={CatalogItem}): result → {Result}", catalogItem, result);

		_logger.LogWarning("<<< Exiting Edit(catalogItem={CatalogItem})", catalogItem);
		_logger.LogError(">>> Entering RefreshLocalStorageList()");
		_logger.LogWarning("<<< Exiting RefreshLocalStorageList()");
        return result;
    }

    public async Task<string> Delete(int id)
    {
		_logger.LogError(">>> Entering Delete(id={Id})", id);
        var result = await _catalogItemService.Delete(id);
        await RefreshLocalStorageList();
		_logger.LogError("Delete(id={Id}): result → {Result}", id, result);

		_logger.LogWarning("<<< Exiting Delete(id={Id})", id);
        return result;
    }

    private async Task RefreshLocalStorageList()
    {
        string key = "items";

        await _localStorageService.RemoveItemAsync(key);
        var items = await _catalogItemService.List();
		_logger.LogError("RefreshLocalStorageList(): items → {Items}", items);
        var entry = new CacheEntry<List<CatalogItem>>(items);
		_logger.LogError("RefreshLocalStorageList(): entry → {Entry}", entry);
        await _localStorageService.SetItemAsync(key, entry);
    }
}
