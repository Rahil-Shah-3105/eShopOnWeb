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
		_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", app);
        app.MapDelete("api/catalog-items/{catalogItemId}",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Supplier security — State change recorded for the audit trail", app);
                return await HandleAsync(new DeleteCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<DeleteCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT [A.8.24]: Cryptography — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(DeleteCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        var response = new DeleteCatalogItemResponse(request.CorrelationId());

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Supplier security — State change recorded for the audit trail", request, itemRepository);
        var itemToDelete = await itemRepository.GetByIdAsync(request.CatalogItemId);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", request, itemRepository);
        if (itemToDelete is null)
            return Results.NotFound();

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", request, itemRepository);
        await itemRepository.DeleteAsync(itemToDelete);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT [A.8.24]: Cryptography — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
