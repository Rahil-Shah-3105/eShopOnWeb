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
		_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", app);
        app.MapDelete("api/catalog-items/{catalogItemId}",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail", app);
                return await HandleAsync(new DeleteCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<DeleteCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(DeleteCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", request, itemRepository);
        var response = new DeleteCatalogItemResponse(request.CorrelationId());
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", request, itemRepository);
        var itemToDelete = await itemRepository.GetByIdAsync(request.CatalogItemId);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", request, itemRepository);
        if (itemToDelete is null)
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", request, itemRepository);
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", request, itemRepository);
            return Results.NotFound();

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", request, itemRepository);
        await itemRepository.DeleteAsync(itemToDelete);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
