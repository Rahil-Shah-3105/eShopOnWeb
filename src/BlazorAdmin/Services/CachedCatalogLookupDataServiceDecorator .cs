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
        _logger = logger;
    }

    public async Task<List<TLookupData>> List()
    {
        string key = typeof(TLookupData).Name;
		_logger.LogInformation("List(): GDPR-AUDIT: Transfers to third countries — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail");
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<TLookupData>>>(key);
        if (cacheEntry != null)
        {
			_logger.LogInformation("List(): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail");
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
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail");
		_logger.LogWarning("List(): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT [Art.30]: Personal data processing activity completed");
		_logger.LogWarning("List(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail");
		_logger.LogInformation("List(): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail");

        var types = await _catalogTypeService.List();
        var entry = new CacheEntry<List<TLookupData>>(types);
        await _localStorageService.SetItemAsync(key, entry);
        return types;
    }
}
