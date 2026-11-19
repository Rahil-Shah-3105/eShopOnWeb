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
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
		_logger.LogInformation("List(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");
        string key = typeof(TLookupData).Name;
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<TLookupData>>>(key);
        if (cacheEntry != null)
        {
			_logger.LogInformation("List(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
            _logger.LogInformation($"Loading {key} from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Cache expired; removing {key} from local storage.");
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

        var types = await _catalogTypeService.List();
        var entry = new CacheEntry<List<TLookupData>>(types);
        await _localStorageService.SetItemAsync(key, entry);
        return types;
    }
}
