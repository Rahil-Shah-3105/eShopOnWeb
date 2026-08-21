using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using MinimalApi.Endpoint;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

/// <summary>
/// Creates a new Catalog Item
/// </summary>
public class CreateCatalogItemEndpoint : IEndpoint<IResult, CreateCatalogItemRequest, IRepository<CatalogItem>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IUriComposer _uriComposer;

    public CreateCatalogItemEndpoint(IUriComposer uriComposer)
    {
		_logger.LogDebug(">>> Entering CreateCatalogItemEndpoint(uriComposer={UriComposer})", uriComposer);
        _uriComposer = uriComposer;
		_logger.LogDebug("CreateCatalogItemEndpoint(uriComposer={UriComposer}): _uriComposer → {_uriComposer}", uriComposer, _uriComposer);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogDebug(">>> Entering AddRoute(app={App})", app);
        app.MapPost("api/catalog-items",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (CreateCatalogItemRequest request, IRepository<CatalogItem> itemRepository) =>
            {
                return await HandleAsync(request, itemRepository);
            })
            .Produces<CreateCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogDebug("<<< Exiting AddRoute(app={App})", app);
    }

    public async Task<IResult> HandleAsync(CreateCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogDebug(">>> Entering HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        var response = new CreateCatalogItemResponse(request.CorrelationId());

		_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): response → {Response}", request, itemRepository, response);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var catalogItemNameSpecification = new CatalogItemNameSpecification(request.Name);
        var existingCataloogItem = await itemRepository.CountAsync(catalogItemNameSpecification);
        if (existingCataloogItem > 0)
        {
			_logger.LogWarning("Throwing DuplicateException from HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
            throw new DuplicateException($"A catalogItem with name {request.Name} already exists");
        }

        var newItem = new CatalogItem(request.CatalogTypeId, request.CatalogBrandId, request.Description, request.Name, request.Price, request.PictureUri);
        newItem = await itemRepository.AddAsync(newItem);

		_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): newItem → {NewItem}", request, itemRepository, newItem);
		_logger.LogInformation("HandleAsync(request,itemRepository)={Request},{ItemRepository}: database query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
        if (newItem.Id != 0)
        {
            //We disabled the upload functionality and added a default/placeholder image to this sample due to a potential security risk 
            //  pointed out by the community. More info in this issue: https://github.com/dotnet-architecture/eShopOnWeb/issues/537 
            //  In production, we recommend uploading to a blob storage and deliver the image via CDN after a verification process.

            newItem.UpdatePictureUri("eCatalog-item-default.png");
            await itemRepository.UpdateAsync(newItem);
			_logger.LogInformation("HandleAsync(request,itemRepository)={Request},{ItemRepository}: update query executed in {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
        }

        var dto = new CatalogItemDto
        {
            Id = newItem.Id,
            CatalogBrandId = newItem.CatalogBrandId,
            CatalogTypeId = newItem.CatalogTypeId,
            Description = newItem.Description,
            Name = newItem.Name,
            PictureUri = _uriComposer.ComposePicUri(newItem.PictureUri),
            Price = newItem.Price
        };
        response.CatalogItem = dto;
		_logger.LogDebug("HandleAsync(request={Request},itemRepository={ItemRepository}): response.CatalogItem → {Arg2}", request, itemRepository, response.CatalogItem);
		_logger.LogDebug("<<< Exiting HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        return Results.Created($"api/catalog-items/{dto.Id}", response);
    }
}
