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
		_logger.LogDebug(">>> Entering CachedCatalogViewModelService(cache={Cache},catalogViewModelService={CatalogViewModelService})", cache, catalogViewModelService);
        _cache = cache;
        _catalogViewModelService = catalogViewModelService;
		_logger.LogDebug("CachedCatalogViewModelService(cache={Cache},catalogViewModelService={CatalogViewModelService}): _cache → {_cache}", cache, catalogViewModelService, _cache);
		_logger.LogDebug("<<< Exiting CachedCatalogViewModelService(cache={Cache},catalogViewModelService={CatalogViewModelService})", cache, catalogViewModelService);
		_logger.LogDebug("CachedCatalogViewModelService(cache={Cache},catalogViewModelService={CatalogViewModelService}): _catalogViewModelService → {_catalogViewModelService}", cache, catalogViewModelService, _catalogViewModelService);
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
		_logger.LogDebug(">>> Entering GetBrands()");
        return (await _cache.GetOrCreateAsync(CacheHelpers.GenerateBrandsCacheKey(), async entry =>
                {
					_logger.LogDebug("GetBrands(): entry.SlidingExpiration → {Arg0}", entry.SlidingExpiration);
                    entry.SlidingExpiration = CacheHelpers.DefaultCacheDuration;
				_logger.LogDebug("<<< Exiting GetBrands()");
                    return await _catalogViewModelService.GetBrands();
                })) ?? new List<SelectListItem>();
    }

    public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {
		_logger.LogDebug(">>> Entering GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId})", pageIndex, itemsPage, brandId, typeId);
        var cacheKey = CacheHelpers.GenerateCatalogItemCacheKey(pageIndex, Constants.ITEMS_PER_PAGE, brandId, typeId);

		_logger.LogDebug("GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId}): cacheKey → {CacheKey}", pageIndex, itemsPage, brandId, typeId, cacheKey);
        return (await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
			_logger.LogDebug("GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId}): entry.SlidingExpiration → {Arg4}", pageIndex, itemsPage, brandId, typeId, entry.SlidingExpiration);
            entry.SlidingExpiration = CacheHelpers.DefaultCacheDuration;
		_logger.LogDebug("<<< Exiting GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId})", pageIndex, itemsPage, brandId, typeId);
            return await _catalogViewModelService.GetCatalogItems(pageIndex, itemsPage, brandId, typeId);
        })) ?? new CatalogIndexViewModel();
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
		_logger.LogDebug(">>> Entering GetTypes()");
        return (await _cache.GetOrCreateAsync(CacheHelpers.GenerateTypesCacheKey(), async entry =>
        {
			_logger.LogDebug("GetTypes(): entry.SlidingExpiration → {Arg0}", entry.SlidingExpiration);
            entry.SlidingExpiration = CacheHelpers.DefaultCacheDuration;
		_logger.LogDebug("<<< Exiting GetTypes()");
            return await _catalogViewModelService.GetTypes();
        })) ?? new List<SelectListItem>();
    }
}
