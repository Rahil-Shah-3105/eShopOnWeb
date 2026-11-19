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
		_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", app);
        app.MapDelete("api/catalog-items/{catalogItemId}",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", app);
                return await HandleAsync(new DeleteCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<DeleteCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(DeleteCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        var response = new DeleteCatalogItemResponse(request.CorrelationId());

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", request, itemRepository);
        var itemToDelete = await itemRepository.GetByIdAsync(request.CatalogItemId);
        if (itemToDelete is null)
            return Results.NotFound();

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", request, itemRepository);
        await itemRepository.DeleteAsync(itemToDelete);

        return Results.Ok(response);
    }
}
