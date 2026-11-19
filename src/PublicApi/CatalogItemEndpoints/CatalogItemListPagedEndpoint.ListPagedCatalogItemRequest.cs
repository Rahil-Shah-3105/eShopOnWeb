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
		_logger.LogError(">>> Entering ListPagedCatalogItemRequest(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId})", pageSize, pageIndex, catalogBrandId, catalogTypeId);
        PageSize = pageSize ?? 0;
        PageIndex = pageIndex ?? 0;
		_logger.LogDebug("ListPagedCatalogItemRequest(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId}): PageIndex → {PageIndex1}", pageSize, pageIndex, catalogBrandId, catalogTypeId, PageIndex);
		_logger.LogDebug("ListPagedCatalogItemRequest(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId}): PageSize → {PageSize1}", pageSize, pageIndex, catalogBrandId, catalogTypeId, PageSize);
    _logger.LogWarning("<<< Exiting ListPagedCatalogItemRequest(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId})", pageSize, pageIndex, catalogBrandId, catalogTypeId);
        CatalogBrandId = catalogBrandId;
		_logger.LogDebug("ListPagedCatalogItemRequest(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId}): CatalogBrandId → {CatalogBrandId1}", pageSize, pageIndex, catalogBrandId, catalogTypeId, CatalogBrandId);
        CatalogTypeId = catalogTypeId;
		_logger.LogDebug("ListPagedCatalogItemRequest(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId}): CatalogTypeId → {CatalogTypeId1}", pageSize, pageIndex, catalogBrandId, catalogTypeId, CatalogTypeId);
    }
}
