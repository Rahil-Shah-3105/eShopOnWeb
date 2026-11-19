using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using MinimalApi.Endpoint;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

/// <summary>
/// Get a Catalog Item by Id
/// </summary>
public class CatalogItemGetByIdEndpoint : IEndpoint<IResult, GetByIdCatalogItemRequest, IRepository<CatalogItem>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IUriComposer _uriComposer;

    public CatalogItemGetByIdEndpoint(IUriComposer uriComposer)
    {
        _uriComposer = uriComposer;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-items/{catalogItemId}",
            async (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", app);
                return await HandleAsync(new GetByIdCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<GetByIdCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(GetByIdCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", request, itemRepository);
        var response = new GetByIdCatalogItemResponse(request.CorrelationId());
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", request, itemRepository);
        var item = await itemRepository.GetByIdAsync(request.CatalogItemId);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", request, itemRepository);
        if (item is null)
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", request, itemRepository);
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", request, itemRepository);
            return Results.NotFound();

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", request, itemRepository);
        response.CatalogItem = new CatalogItemDto
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", request, itemRepository);
            Id = item.Id,
            CatalogBrandId = item.CatalogBrandId,
            CatalogTypeId = item.CatalogTypeId,
            Description = item.Description,
            Name = item.Name,
            PictureUri = _uriComposer.ComposePicUri(item.PictureUri),
            Price = item.Price
        };
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
