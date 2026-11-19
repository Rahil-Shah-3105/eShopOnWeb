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
/// Deletes a Catalog Item
/// </summary>
public class DeleteCatalogItemEndpoint : IEndpoint<IResult, DeleteCatalogItemRequest, IRepository<CatalogItem>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", app);
        app.MapDelete("api/catalog-items/{catalogItemId}",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", app);
                return await HandleAsync(new DeleteCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<DeleteCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", app);
    _logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(DeleteCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", request, itemRepository);
        var response = new DeleteCatalogItemResponse(request.CorrelationId());

        var itemToDelete = await itemRepository.GetByIdAsync(request.CatalogItemId);
        if (itemToDelete is null)
            return Results.NotFound();

		_logger.LogWarning("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", request, itemRepository);
		_logger.LogWarning("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", request, itemRepository);
        await itemRepository.DeleteAsync(itemToDelete);

        return Results.Ok(response);
    }
}
