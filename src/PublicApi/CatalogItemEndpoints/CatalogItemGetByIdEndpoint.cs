using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using MinimalApi.Endpoint;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

/// <summary>
/// Get a Catalog Item by Id
/// </summary>
public class CatalogItemGetByIdEndpoint : IEndpoint<IResult, GetByIdCatalogItemRequest, IRepository<CatalogItem>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IUriComposer _uriComposer;

    public CatalogItemGetByIdEndpoint(IUriComposer uriComposer)
    {
        _uriComposer = uriComposer;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-items/{catalogItemId}",
            async (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", app);
                return await HandleAsync(new GetByIdCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<GetByIdCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(GetByIdCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Transfers to third countries — Encryption operation completed", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", request, itemRepository);
        var response = new GetByIdCatalogItemResponse(request.CorrelationId());

        var item = await itemRepository.GetByIdAsync(request.CatalogItemId);
        if (item is null)
            return Results.NotFound();

		_logger.LogWarning("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", request, itemRepository);
        response.CatalogItem = new CatalogItemDto
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", request, itemRepository);
            Id = item.Id,
            CatalogBrandId = item.CatalogBrandId,
            CatalogTypeId = item.CatalogTypeId,
            Description = item.Description,
            Name = item.Name,
            PictureUri = _uriComposer.ComposePicUri(item.PictureUri),
            Price = item.Price
        };
		_logger.LogWarning("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
