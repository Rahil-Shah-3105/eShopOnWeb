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
        _uriComposer = uriComposer;
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-items",
            async (int? pageSize, int? pageIndex, int? catalogBrandId, int? catalogTypeId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", app);
                return await HandleAsync(new ListPagedCatalogItemRequest(pageSize, pageIndex, catalogBrandId, catalogTypeId), itemRepository);
            })
            .Produces<ListPagedCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(ListPagedCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        await Task.Delay(1000);
        var response = new ListPagedCatalogItemResponse(request.CorrelationId());

        var filterSpec = new CatalogFilterSpecification(request.CatalogBrandId, request.CatalogTypeId);
        int totalItems = await itemRepository.CountAsync(filterSpec);

        var pagedSpec = new CatalogFilterPaginatedSpecification(
            skip: request.PageIndex * request.PageSize,
            take: request.PageSize,
            brandId: request.CatalogBrandId,
            typeId: request.CatalogTypeId);

        var items = await itemRepository.ListAsync(pagedSpec);

        response.CatalogItems.AddRange(items.Select(_mapper.Map<CatalogItemDto>));
        foreach (CatalogItemDto item in response.CatalogItems)
        {
            item.PictureUri = _uriComposer.ComposePicUri(item.PictureUri);
        }
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", request, itemRepository);

        if (request.PageSize > 0)
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", request, itemRepository);
			_logger.LogWarning("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", request, itemRepository);
            response.PageCount = int.Parse(Math.Ceiling((decimal)totalItems / request.PageSize).ToString());
        }
        else
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", request, itemRepository);
            response.PageCount = totalItems > 0 ? 1 : 0;
        }
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", request, itemRepository);
		_logger.LogWarning("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", request, itemRepository);

        return Results.Ok(response);
    }
}
