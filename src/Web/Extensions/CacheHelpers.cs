using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.Web.Extensions;

public static class CacheHelpers
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
    private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}-{3}";

    public static string GenerateCatalogItemCacheKey(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {
		_logger.LogWarning("GenerateCatalogItemCacheKey(pageIndex,itemsPage,brandId,typeId)={PageIndex},{ItemsPage},{BrandId},{TypeId}: NERCCIP-AUDIT: Security incident detected", pageIndex, itemsPage, brandId, typeId);
        return string.Format(_itemsKeyTemplate, pageIndex, itemsPage, brandId, typeId);
    }

    public static string GenerateBrandsCacheKey()
    {
        return "brands";
    }

    public static string GenerateTypesCacheKey()
    {
        return "types";
    }
}
