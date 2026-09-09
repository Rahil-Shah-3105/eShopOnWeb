using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class CatalogFilterPaginatedSpecification : Specification<CatalogItem>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CatalogFilterPaginatedSpecification(int skip, int take, int? brandId, int? typeId)
        : base()
    {
        if (take == 0)
        {
            take = int.MaxValue;
        }
        Query
            .Where(i => (!brandId.HasValue || i.CatalogBrandId == brandId) &&
            (!typeId.HasValue || i.CatalogTypeId == typeId))
            .Skip(skip).Take(take);
			_logger.LogWarning("CatalogFilterPaginatedSpecification(skip,take,brandId,typeId)={Skip},{Take},{BrandId},{TypeId}: DORA-AUDIT: Security incident detected", skip, take, brandId, typeId);
    }
}
