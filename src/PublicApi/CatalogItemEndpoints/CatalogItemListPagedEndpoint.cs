using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using MinimalApi.Endpoint;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

/// <summary>
/// List Catalog Items (paged)
/// </summary>
public class CatalogItemListPagedEndpoint : IEndpoint<IResult, ListPagedCatalogItemRequest, IRepository<CatalogItem>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IUriComposer _uriComposer;
    private readonly IMapper _mapper;

    public CatalogItemListPagedEndpoint(IUriComposer uriComposer, IMapper mapper)
    {
		_logger.LogError(">>> Entering CatalogItemListPagedEndpoint(uriComposer={UriComposer},mapper={Mapper})", uriComposer, mapper);
        _uriComposer = uriComposer;
        _mapper = mapper;
		_logger.LogDebug("CatalogItemListPagedEndpoint(uriComposer={UriComposer},mapper={Mapper}): _uriComposer → {_uriComposer}", uriComposer, mapper, _uriComposer);
    _logger.LogWarning("<<< Exiting CatalogItemListPagedEndpoint(uriComposer={UriComposer},mapper={Mapper})", uriComposer, mapper);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogError(">>> Entering AddRoute(app={App})", app);
        app.MapGet("api/catalog-items",
            async (int? pageSize, int? pageIndex, int? catalogBrandId, int? catalogTypeId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogError(">>> Entering async(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId},itemRepository={ItemRepository})", pageSize, pageIndex, catalogBrandId, catalogTypeId, itemRepository);
                _logger.LogWarning("<<< Exiting async(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId},itemRepository={ItemRepository})", pageSize, pageIndex, catalogBrandId, catalogTypeId, itemRepository);
			_logger.LogWarning("<<< Exiting AddRoute(app={App})", app);
                return await HandleAsync(new ListPagedCatalogItemRequest(pageSize, pageIndex, catalogBrandId, catalogTypeId), itemRepository);
            })
            .Produces<ListPagedCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
    }

    public async Task<IResult> HandleAsync(ListPagedCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogError(">>> Entering HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        await Task.Delay(1000);
        var response = new ListPagedCatalogItemResponse(request.CorrelationId());
		_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): response → {Response}", request, itemRepository, response);

        var filterSpec = new CatalogFilterSpecification(request.CatalogBrandId, request.CatalogTypeId);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        int totalItems = await itemRepository.CountAsync(filterSpec);

		_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): filterSpec → {FilterSpec}", request, itemRepository, filterSpec);
        var pagedSpec = new CatalogFilterPaginatedSpecification(
            skip: request.PageIndex * request.PageSize,
            take: request.PageSize,
            brandId: request.CatalogBrandId,
            typeId: request.CatalogTypeId);
			_logger.LogError("HandleAsync(request,itemRepository)={Request},{ItemRepository}: count query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
			_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): totalItems → {TotalItems}", request, itemRepository, totalItems);

        var items = await itemRepository.ListAsync(pagedSpec);

		_logger.LogError("HandleAsync(request,itemRepository)={Request},{ItemRepository}: database query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
		_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): items → {Items}", request, itemRepository, items);
        response.CatalogItems.AddRange(items.Select(_mapper.Map<CatalogItemDto>));
        foreach (CatalogItemDto item in response.CatalogItems)
        {
            item.PictureUri = _uriComposer.ComposePicUri(item.PictureUri);
        }

        if (request.PageSize > 0)
        {
            response.PageCount = int.Parse(Math.Ceiling((decimal)totalItems / request.PageSize).ToString());
        }
        else
        {
            response.PageCount = totalItems > 0 ? 1 : 0;
			_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): response.PageCount → {Arg2}", request, itemRepository, response.PageCount);
        }

		_logger.LogWarning("<<< Exiting HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        return Results.Ok(response);
    }
}
