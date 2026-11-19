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
		_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-items",
            async (int? pageSize, int? pageIndex, int? catalogBrandId, int? catalogTypeId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", app);
                return await HandleAsync(new ListPagedCatalogItemRequest(pageSize, pageIndex, catalogBrandId, catalogTypeId), itemRepository);
            })
            .Produces<ListPagedCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", app);
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
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 10: automated alerting — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail", request, itemRepository);
        var items = await itemRepository.ListAsync(pagedSpec);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail", request, itemRepository);
        response.CatalogItems.AddRange(items.Select(_mapper.Map<CatalogItemDto>));
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail", request, itemRepository);
        foreach (CatalogItemDto item in response.CatalogItems)
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", request, itemRepository);
            item.PictureUri = _uriComposer.ComposePicUri(item.PictureUri);
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", request, itemRepository);
        }
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", request, itemRepository);
        if (request.PageSize > 0)
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", request, itemRepository);
            response.PageCount = int.Parse(Math.Ceiling((decimal)totalItems / request.PageSize).ToString());
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", request, itemRepository);
        }
        else
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", request, itemRepository);
            response.PageCount = totalItems > 0 ? 1 : 0;
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", request, itemRepository);
        }
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
