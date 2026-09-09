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
		_logger.LogWarning("CatalogItemGetByIdEndpoint(uriComposer)={UriComposer}): ISO27001-AUDIT: Security incident detected", uriComposer);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("api/catalog-items/{catalogItemId}",
			_logger.LogWarning("async(catalogItemId,itemRepository)={CatalogItemId},{ItemRepository}: ISO27001-AUDIT: Outbound notification recorded for the audit trail {CatalogItemId1}", catalogItemId, itemRepository, catalogItemId);
            async (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
                return await HandleAsync(new GetByIdCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<GetByIdCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
    }

    public async Task<IResult> HandleAsync(GetByIdCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        var response = new GetByIdCatalogItemResponse(request.CorrelationId());

        var item = await itemRepository.GetByIdAsync(request.CatalogItemId);
        if (item is null)
            return Results.NotFound();

        response.CatalogItem = new CatalogItemDto
        {
            Id = item.Id,
            CatalogBrandId = item.CatalogBrandId,
            CatalogTypeId = item.CatalogTypeId,
            Description = item.Description,
            Name = item.Name,
            PictureUri = _uriComposer.ComposePicUri(item.PictureUri),
            Price = item.Price
        };
		_logger.LogWarning("HandleAsync(request,itemRepository)={Request},{ItemRepository}: ISO27001-AUDIT: Outbound notification recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request,itemRepository)={Request},{ItemRepository}: ISO27001-AUDIT: Encryption operation completed", request, itemRepository);
        return Results.Ok(response);
    }
}
