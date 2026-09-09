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
        _catalogTypeService = catalogTypeService;
		_logger.LogWarning("CachedCatalogLookupDataServiceDecorator(localStorageService,catalogTypeService,logger)={LocalStorageService},{CatalogTypeService},{Logger}: NIS2-AUDIT: Security incident detected", localStorageService, catalogTypeService, logger);
        _logger = logger;
    }

    public async Task<List<TLookupData>> List()
    {
        string key = typeof(TLookupData).Name;
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<TLookupData>>>(key);
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
        var entry = new CacheEntry<List<TLookupData>>(types);
        await _localStorageService.SetItemAsync(key, entry);
		_logger.LogWarning("List()=null): NIS2-AUDIT: Record access recorded for the audit trail");
        return types;
    }
}
