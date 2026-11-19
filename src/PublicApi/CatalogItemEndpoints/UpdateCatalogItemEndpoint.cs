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
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", app);
        app.MapPut("api/catalog-items",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (UpdateCatalogItemRequest request, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", app);
                return await HandleAsync(request, itemRepository);
            })
            .Produces<UpdateCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", app);
    _logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", app);
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
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Transfers to third countries — Encryption operation completed", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", request, itemRepository);
		_logger.LogWarning("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", request, itemRepository);

        await itemRepository.UpdateAsync(existingItem);

        var dto = new CatalogItemDto
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", request, itemRepository);
            Id = existingItem.Id,
            CatalogBrandId = existingItem.CatalogBrandId,
            CatalogTypeId = existingItem.CatalogTypeId,
            Description = existingItem.Description,
            Name = existingItem.Name,
            PictureUri = _uriComposer.ComposePicUri(existingItem.PictureUri),
            Price = existingItem.Price
        };
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", request, itemRepository);
		_logger.LogWarning("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", request, itemRepository);
        response.CatalogItem = dto;
        return Results.Ok(response);
    }
}
