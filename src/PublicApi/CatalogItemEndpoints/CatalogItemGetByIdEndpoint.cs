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
		_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Supplier security — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-items/{catalogItemId}",
            async (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", app);
                return await HandleAsync(new GetByIdCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<GetByIdCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT [A.8.24]: Cryptography — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(GetByIdCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        var response = new GetByIdCatalogItemResponse(request.CorrelationId());

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Supplier security — State change recorded for the audit trail", request, itemRepository);
        var item = await itemRepository.GetByIdAsync(request.CatalogItemId);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", request, itemRepository);
        if (item is null)
            return Results.NotFound();

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", request, itemRepository);
        response.CatalogItem = new CatalogItemDto
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT [A.8.24]: Cryptography — Encryption operation completed", request, itemRepository);
            Id = item.Id,
            CatalogBrandId = item.CatalogBrandId,
            CatalogTypeId = item.CatalogTypeId,
            Description = item.Description,
            Name = item.Name,
            PictureUri = _uriComposer.ComposePicUri(item.PictureUri),
            Price = item.Price
        };
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
