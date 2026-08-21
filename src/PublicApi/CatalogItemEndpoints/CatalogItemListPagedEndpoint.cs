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
		_logger.LogDebug(">>> Entering CatalogItemListPagedEndpoint(uriComposer={UriComposer},mapper={Mapper})", uriComposer, mapper);
        _uriComposer = uriComposer;
		_logger.LogDebug("CatalogItemListPagedEndpoint(uriComposer={UriComposer},mapper={Mapper}): _uriComposer → {_uriComposer}", uriComposer, mapper, _uriComposer);
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogDebug(">>> Entering AddRoute(app={App})", app);
        app.MapGet("api/catalog-items",
			_logger.LogDebug(">>> Entering async(pageSize={PageSize},pageIndex={PageIndex},catalogBrandId={CatalogBrandId},catalogTypeId={CatalogTypeId},itemRepository={ItemRepository})", pageSize, pageIndex, catalogBrandId, catalogTypeId, itemRepository);
            async (int? pageSize, int? pageIndex, int? catalogBrandId, int? catalogTypeId, IRepository<CatalogItem> itemRepository) =>
            {
                return await HandleAsync(new ListPagedCatalogItemRequest(pageSize, pageIndex, catalogBrandId, catalogTypeId), itemRepository);
            })
            .Produces<ListPagedCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
    }

    public async Task<IResult> HandleAsync(ListPagedCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogDebug(">>> Entering HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        await Task.Delay(1000);
        var response = new ListPagedCatalogItemResponse(request.CorrelationId());
		_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): response → {Response}", request, itemRepository, response);

        var filterSpec = new CatalogFilterSpecification(request.CatalogBrandId, request.CatalogTypeId);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        int totalItems = await itemRepository.CountAsync(filterSpec);

		_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): filterSpec → {FilterSpec}", request, itemRepository, filterSpec);
        var pagedSpec = new CatalogFilterPaginatedSpecification(
            skip: request.PageIndex * request.PageSize,
            take: request.PageSize,
            brandId: request.CatalogBrandId,
            typeId: request.CatalogTypeId);
			_logger.LogInformation("HandleAsync(request,itemRepository)={Request},{ItemRepository}: count query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
			_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): totalItems → {TotalItems}", request, itemRepository, totalItems);

        var items = await itemRepository.ListAsync(pagedSpec);

		_logger.LogInformation("HandleAsync(request,itemRepository)={Request},{ItemRepository}: database query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): items → {Items}", request, itemRepository, items);
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

		_logger.LogDebug("<<< Exiting HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        return Results.Ok(response);
    }
}
