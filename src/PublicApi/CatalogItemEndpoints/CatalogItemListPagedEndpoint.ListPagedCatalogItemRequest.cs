using Microsoft.Extensions.Logging;
namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class ListPagedCatalogItemRequest : BaseRequest
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public int PageSize { get; init; }
    public int PageIndex { get; init; }
    public int? CatalogBrandId { get; init; }
    public int? CatalogTypeId { get; init; }

    public ListPagedCatalogItemRequest(int? pageSize, int? pageIndex, int? catalogBrandId, int? catalogTypeId)
    {
        PageSize = pageSize ?? 0;
        PageIndex = pageIndex ?? 0;
        CatalogBrandId = catalogBrandId;
        CatalogTypeId = catalogTypeId;
		_logger.LogWarning("ListPagedCatalogItemRequest(pageSize,pageIndex,catalogBrandId,catalogTypeId)={PageSize},{PageIndex},{CatalogBrandId},{CatalogTypeId}: DORA-AUDIT: Security incident detected", pageSize, pageIndex, catalogBrandId, catalogTypeId);
    }
}
