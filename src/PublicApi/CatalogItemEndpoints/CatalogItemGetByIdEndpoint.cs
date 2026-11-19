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
		_logger.LogError(">>> Entering CatalogItemGetByIdEndpoint(uriComposer={UriComposer})", uriComposer);
        _uriComposer = uriComposer;
		_logger.LogDebug("CatalogItemGetByIdEndpoint(uriComposer={UriComposer}): _uriComposer → {_uriComposer}", uriComposer, _uriComposer);
    _logger.LogWarning("<<< Exiting CatalogItemGetByIdEndpoint(uriComposer={UriComposer})", uriComposer);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogError(">>> Entering AddRoute(app={App})", app);
        app.MapGet("api/catalog-items/{catalogItemId}",
            async (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogError(">>> Entering async(catalogItemId={CatalogItemId},itemRepository={ItemRepository})", catalogItemId, itemRepository);
                _logger.LogWarning("<<< Exiting async(catalogItemId={CatalogItemId},itemRepository={ItemRepository})", catalogItemId, itemRepository);
			_logger.LogWarning("<<< Exiting AddRoute(app={App})", app);
                return await HandleAsync(new GetByIdCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<GetByIdCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
    }

    public async Task<IResult> HandleAsync(GetByIdCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogError(">>> Entering HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        var response = new GetByIdCatalogItemResponse(request.CorrelationId());

		var __logStart = System.Diagnostics.Stopwatch.StartNew();
		_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): response → {Response}", request, itemRepository, response);
		_logger.LogError("HandleAsync(request,itemRepository)={Request},{ItemRepository}: find query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
        var item = await itemRepository.GetByIdAsync(request.CatalogItemId);
			_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): item → {Item}", request, itemRepository, item);
        if (item is null) {
            return Results.NotFound();
        }

        response.CatalogItem = new CatalogItemDto
        {
			_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): response.CatalogItem → {Arg2}", request, itemRepository, response.CatalogItem);
            Id = item.Id,
            CatalogBrandId = item.CatalogBrandId,
            CatalogTypeId = item.CatalogTypeId,
            Description = item.Description,
            Name = item.Name,
            PictureUri = _uriComposer.ComposePicUri(item.PictureUri),
            Price = item.Price
        };
		_logger.LogWarning("<<< Exiting HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        return Results.Ok(response);
    }
}
