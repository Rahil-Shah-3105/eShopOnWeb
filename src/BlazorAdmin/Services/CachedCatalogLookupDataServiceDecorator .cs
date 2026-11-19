using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Services;

public class CachedCatalogLookupDataServiceDecorator<TLookupData, TReponse>
    : ICatalogLookupDataService<TLookupData>
    where TLookupData : LookupData
    where TReponse : ILookupDataResponse<TLookupData>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly ILocalStorageService _localStorageService;
    private readonly CatalogLookupDataService<TLookupData, TReponse> _catalogTypeService;
    private ILogger<CachedCatalogLookupDataServiceDecorator<TLookupData, TReponse>> _logger;

    public CachedCatalogLookupDataServiceDecorator(ILocalStorageService localStorageService,
        CatalogLookupDataService<TLookupData, TReponse> catalogTypeService,
        ILogger<CachedCatalogLookupDataServiceDecorator<TLookupData, TReponse>> logger)
    {
        _localStorageService = localStorageService;
		_logger.LogDebug("CachedCatalogLookupDataServiceDecorator(localStorageService={LocalStorageService},catalogTypeService={CatalogTypeService},logger={Logger}): _localStorageService → {_localStorageService}", localStorageService, catalogTypeService, logger, _localStorageService);
        _catalogTypeService = catalogTypeService;
		_logger.LogDebug("CachedCatalogLookupDataServiceDecorator(localStorageService={LocalStorageService},catalogTypeService={CatalogTypeService},logger={Logger}): _catalogTypeService → {_catalogTypeService}", localStorageService, catalogTypeService, logger, _catalogTypeService);
        _logger = logger;
		_logger.LogError(">>> Entering CachedCatalogLookupDataServiceDecorator(localStorageService={LocalStorageService},catalogTypeService={CatalogTypeService},logger={Logger})", localStorageService, catalogTypeService, logger);
    _logger.LogWarning("<<< Exiting CachedCatalogLookupDataServiceDecorator(localStorageService={LocalStorageService},catalogTypeService={CatalogTypeService},logger={Logger})", localStorageService, catalogTypeService, logger);
    }

    public async Task<List<TLookupData>> List()
    {
		_logger.LogError(">>> Entering List()");
        string key = typeof(TLookupData).Name;
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<TLookupData>>>(key);
		_logger.LogError("List(): cacheEntry → {CacheEntry}", cacheEntry);
        if (cacheEntry != null)
        {
            _logger.LogInformation($"Loading {key} from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Cache expired; removing {key} from local storage.");
                await _localStorageService.RemoveItemAsync(key);
            }
        }

        var types = await _catalogTypeService.List();
		_logger.LogError("List(): types → {Types}", types);
        var entry = new CacheEntry<List<TLookupData>>(types);
		_logger.LogError("List(): entry → {Entry}", entry);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogWarning("<<< Exiting List()");
        return types;
    }
}
