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
		_logger.LogError(">>> Entering AddRoute(app={App})", app);
        app.MapDelete("api/catalog-items/{catalogItemId}",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
			_logger.LogWarning("<<< Exiting AddRoute(app={App})", app);
                return await HandleAsync(new DeleteCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<DeleteCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
    }

    public async Task<IResult> HandleAsync(DeleteCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogError(">>> Entering HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var response = new DeleteCatalogItemResponse(request.CorrelationId());

		var __logStartDb = System.Diagnostics.Stopwatch.StartNew();
        var itemToDelete = await itemRepository.GetByIdAsync(request.CatalogItemId);
			_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): response → {Response}", request, itemRepository, response);
			_logger.LogError("HandleAsync(request,itemRepository)={Request},{ItemRepository}: find query executed in {Arg2} ms", request, itemRepository, __logStartDb.ElapsedMilliseconds);
			_logger.LogError("HandleAsync(request={Request},itemRepository={ItemRepository}): itemToDelete → {ItemToDelete}", request, itemRepository, itemToDelete);
			_logger.LogError("HandleAsync(request,itemRepository)={Request},{ItemRepository}: delete query executed in {Arg2} ms", request, itemRepository, __logStartDb.ElapsedMilliseconds);
        if (itemToDelete is null) {
            return Results.NotFound();
        }

        await itemRepository.DeleteAsync(itemToDelete);

		_logger.LogError("HandleAsync(request,itemRepository)={Request},{ItemRepository}: external service call took {Arg2} ms", request, itemRepository, __logStart.ElapsedMilliseconds);
		_logger.LogWarning("<<< Exiting HandleAsync(request={Request},itemRepository={ItemRepository})", request, itemRepository);
        return Results.Ok(response);
    }
}
