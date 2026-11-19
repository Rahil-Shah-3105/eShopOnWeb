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
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail", pageSize);
        if (cacheEntry != null)
        {
			_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT: Transfers to third countries — Record access recorded for the audit trail", pageSize);
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
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail", pageSize);
		_logger.LogWarning("ListPaged(pageSize={PageSize}): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail", pageSize);
		_logger.LogWarning("ListPaged(pageSize={PageSize}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail", pageSize);
		_logger.LogInformation("ListPaged(pageSize={PageSize}): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail", pageSize);

        var items = await _catalogItemService.ListPaged(pageSize);
        var entry = new CacheEntry<List<CatalogItem>>(items);
        await _localStorageService.SetItemAsync(key, entry);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
        string key = "items";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail");
        if (cacheEntry != null)
        {
			_logger.LogInformation("List(): GDPR-AUDIT: Transfers to third countries — Record access recorded for the audit trail");
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
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail");
		_logger.LogWarning("List(): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail");
		_logger.LogWarning("List(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail");

        var items = await _catalogItemService.List();
        var entry = new CacheEntry<List<CatalogItem>>(items);
        await _localStorageService.SetItemAsync(key, entry);
        return items;
    }

    public async Task<CatalogItem> GetById(int id)
    {
		_logger.LogWarning("GetById(id={Id}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail {Id1}", id, id);
		_logger.LogWarning("GetById(id={Id}): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("GetById(id={Id}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("GetById(id={Id}): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail {Id1}", id, id);
        return (await List()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
        var result = await _catalogItemService.Create(catalogItem);
		_logger.LogWarning("Create(catalogItem={CatalogItem}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogWarning("Create(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Create(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {Result}", catalogItem, result);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail {Result}", catalogItem, result);
    _logger.LogInformation("Create(catalogItem={CatalogItem}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {Result}", catalogItem, result);
        await RefreshLocalStorageList();

        return result;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
        var result = await _catalogItemService.Edit(catalogItem);
		_logger.LogWarning("Edit(catalogItem={CatalogItem}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogWarning("Edit(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {Result}", catalogItem, result);
		_logger.LogInformation("Edit(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {Result}", catalogItem, result);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail {Result}", catalogItem, result);
    _logger.LogInformation("Edit(catalogItem={CatalogItem}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {Result}", catalogItem, result);
        await RefreshLocalStorageList();

        return result;
    }

    public async Task<string> Delete(int id)
    {
		_logger.LogInformation("Delete(id={Id}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogWarning("Delete(id={Id}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("Delete(id={Id}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogWarning("Delete(id={Id}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {Id1}", id, id);
		_logger.LogInformation("Delete(id={Id}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("Delete(id={Id}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail {Id1}", id, id);
    _logger.LogInformation("Delete(id={Id}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {Id1}", id, id);
        var result = await _catalogItemService.Delete(id);
        await RefreshLocalStorageList();

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
