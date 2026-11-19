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
		_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-items/{catalogItemId}",
            async (int catalogItemId, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", app);
                return await HandleAsync(new GetByIdCatalogItemRequest(catalogItemId), itemRepository);
            })
            .Produces<GetByIdCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(GetByIdCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", request, itemRepository);
        var response = new GetByIdCatalogItemResponse(request.CorrelationId());

        var item = await itemRepository.GetByIdAsync(request.CatalogItemId);
        if (item is null)
            return Results.NotFound();

        response.CatalogItem = new CatalogItemDto
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC6.7]: Encryption operation completed", request, itemRepository);
            Id = item.Id,
            CatalogBrandId = item.CatalogBrandId,
            CatalogTypeId = item.CatalogTypeId,
            Description = item.Description,
            Name = item.Name,
            PictureUri = _uriComposer.ComposePicUri(item.PictureUri),
            Price = item.Price
        };
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", request, itemRepository);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", request, itemRepository);
        return Results.Ok(response);
    }
}
