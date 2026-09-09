using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.Web.Extensions;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Services;

public class CachedCatalogViewModelService : ICatalogViewModelService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IMemoryCache _cache;
    private readonly CatalogViewModelService _catalogViewModelService;

    public CachedCatalogViewModelService(IMemoryCache cache,
        CatalogViewModelService catalogViewModelService)
    {
        _cache = cache;
        _catalogViewModelService = catalogViewModelService;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
		_logger.LogWarning("GetBrands()=null): TRAIGA-AUDIT: Record access recorded for the audit trail");
        return (await _cache.GetOrCreateAsync(CacheHelpers.GenerateBrandsCacheKey(), async entry =>
                {
                    entry.SlidingExpiration = CacheHelpers.DefaultCacheDuration;
                    return await _catalogViewModelService.GetBrands();
                })) ?? new List<SelectListItem>();
    }

    public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {
        var cacheKey = CacheHelpers.GenerateCatalogItemCacheKey(pageIndex, Constants.ITEMS_PER_PAGE, brandId, typeId);

		_logger.LogWarning("GetCatalogItems(pageIndex,itemsPage,brandId,typeId)={PageIndex},{ItemsPage},{BrandId},{TypeId}: TRAIGA-AUDIT: Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        return (await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = CacheHelpers.DefaultCacheDuration;
            return await _catalogViewModelService.GetCatalogItems(pageIndex, itemsPage, brandId, typeId);
        })) ?? new CatalogIndexViewModel();
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
		_logger.LogWarning("GetTypes()=null): TRAIGA-AUDIT: Record access recorded for the audit trail");
        return (await _cache.GetOrCreateAsync(CacheHelpers.GenerateTypesCacheKey(), async entry =>
        {
            entry.SlidingExpiration = CacheHelpers.DefaultCacheDuration;
            return await _catalogViewModelService.GetTypes();
        })) ?? new List<SelectListItem>();
    }
}
