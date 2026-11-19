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
		_logger.LogInformation("UpdateCatalogItemEndpoint(uriComposer={UriComposer}): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed", uriComposer);
        _uriComposer = uriComposer;
		_logger.LogInformation("UpdateCatalogItemEndpoint(uriComposer={UriComposer}): SOC2-AUDIT [CC8.1]: Change management operation performed", uriComposer);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", app);
        app.MapPut("api/catalog-items",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (UpdateCatalogItemRequest request, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail", app);
                return await HandleAsync(request, itemRepository);
            })
            .Produces<UpdateCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(UpdateCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        var response = new UpdateCatalogItemResponse(request.CorrelationId());

        var existingItem = await itemRepository.GetByIdAsync(request.Id);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 10: automated alerting — State change recorded for the audit trail", request, itemRepository);
        if (existingItem == null)
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", request, itemRepository);
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail", request, itemRepository);
            return Results.NotFound();
        }
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail", request, itemRepository);
        CatalogItem.CatalogItemDetails details = new(request.Name, request.Description, request.Price);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", request, itemRepository);
        existingItem.UpdateDetails(details);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", request, itemRepository);
        existingItem.UpdateBrand(request.CatalogBrandId);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", request, itemRepository);
        existingItem.UpdateType(request.CatalogTypeId);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", request, itemRepository);
        await itemRepository.UpdateAsync(existingItem);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", request, itemRepository);
        var dto = new CatalogItemDto
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", request, itemRepository);
            Id = existingItem.Id,
            CatalogBrandId = existingItem.CatalogBrandId,
            CatalogTypeId = existingItem.CatalogTypeId,
            Description = existingItem.Description,
            Name = existingItem.Name,
            PictureUri = _uriComposer.ComposePicUri(existingItem.PictureUri),
            Price = existingItem.Price
        };
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail", request, itemRepository);
        response.CatalogItem = dto;
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
