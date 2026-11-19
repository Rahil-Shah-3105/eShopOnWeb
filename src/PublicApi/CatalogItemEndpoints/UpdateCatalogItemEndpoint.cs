using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using MinimalApi.Endpoint;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

/// <summary>
/// Updates a Catalog Item
/// </summary>
public class UpdateCatalogItemEndpoint : IEndpoint<IResult, UpdateCatalogItemRequest, IRepository<CatalogItem>>
{ 
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IUriComposer _uriComposer;

    public UpdateCatalogItemEndpoint(IUriComposer uriComposer)
    {
		_logger.LogError(">>> Entering UpdateCatalogItemEndpoint(uriComposer={UriComposer})", uriComposer);
        _uriComposer = uriComposer;
		_logger.LogDebug("UpdateCatalogItemEndpoint(uriComposer={UriComposer}): _uriComposer → {_uriComposer}", uriComposer, _uriComposer);
    _logger.LogWarning("<<< Exiting UpdateCatalogItemEndpoint(uriComposer={UriComposer})", uriComposer);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogError(">>> Entering AddRoute(app={App})", app);
        app.MapPut("api/catalog-items",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (UpdateCatalogItemRequest request, IRepository<CatalogItem> itemRepository) =>
            {
			_logger.LogWarning("<<< Exiting AddRoute(app={App})", app);
                return await HandleAsync(request, itemRepository);
            })
            .Produces<UpdateCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
    }

    public async Task<IResult> HandleAsync(UpdateCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogError(">>> Entering HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        var response = new UpdateCatalogItemResponse(request.CorrelationId());

		var __logStart = System.Diagnostics.Stopwatch.StartNew();
		_logger.LogError("HandleAsync(request,itemRepository)={Request},{ItemRepository}: find query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
        var existingItem = await itemRepository.GetByIdAsync(request.Id);
        if (existingItem == null)
        {
			_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): response → {Response}", request, itemRepository, response);
			_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): existingItem → {ExistingItem}", request, itemRepository, existingItem);
            return Results.NotFound();
        }

        CatalogItem.CatalogItemDetails details = new(request.Name, request.Description, request.Price);
		_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): details → {Details}", request, itemRepository, details);
        existingItem.UpdateDetails(details);
        existingItem.UpdateBrand(request.CatalogBrandId);
        existingItem.UpdateType(request.CatalogTypeId);

        await itemRepository.UpdateAsync(existingItem);
		_logger.LogError("HandleAsync(request,itemRepository)={Request},{ItemRepository}: update query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);

        var dto = new CatalogItemDto
        {
            Id = existingItem.Id,
            CatalogBrandId = existingItem.CatalogBrandId,
            CatalogTypeId = existingItem.CatalogTypeId,
            Description = existingItem.Description,
            Name = existingItem.Name,
            PictureUri = _uriComposer.ComposePicUri(existingItem.PictureUri),
            Price = existingItem.Price
        };
        response.CatalogItem = dto;
		_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): response.CatalogItem → {Arg2}", request, itemRepository, response.CatalogItem);
		_logger.LogWarning("<<< Exiting HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        return Results.Ok(response);
    }
}
