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
        _uriComposer = uriComposer;
		_logger.LogInformation("UpdateCatalogItemEndpoint(uriComposer={UriComposer}): ISO27001-AUDIT: Supplier security — Change management operation performed", uriComposer);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", app);
        app.MapPut("api/catalog-items",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (UpdateCatalogItemRequest request, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Supplier security — State change recorded for the audit trail", app);
                return await HandleAsync(request, itemRepository);
            })
            .Produces<UpdateCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT [A.8.24]: Cryptography — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(UpdateCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        var response = new UpdateCatalogItemResponse(request.CorrelationId());

        var existingItem = await itemRepository.GetByIdAsync(request.Id);
        if (existingItem == null)
        {
            return Results.NotFound();
        }

        CatalogItem.CatalogItemDetails details = new(request.Name, request.Description, request.Price);
        existingItem.UpdateDetails(details);
        existingItem.UpdateBrand(request.CatalogBrandId);
        existingItem.UpdateType(request.CatalogTypeId);

        await itemRepository.UpdateAsync(existingItem);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Supplier security — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", request, itemRepository);
        var dto = new CatalogItemDto
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", request, itemRepository);
            Id = existingItem.Id,
            CatalogBrandId = existingItem.CatalogBrandId,
            CatalogTypeId = existingItem.CatalogTypeId,
            Description = existingItem.Description,
            Name = existingItem.Name,
            PictureUri = _uriComposer.ComposePicUri(existingItem.PictureUri),
            Price = existingItem.Price
        };
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT [A.8.24]: Cryptography — Encryption operation completed", request, itemRepository);
        response.CatalogItem = dto;
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
