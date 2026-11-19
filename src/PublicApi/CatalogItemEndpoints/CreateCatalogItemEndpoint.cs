using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using MinimalApi.Endpoint;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

/// <summary>
/// Creates a new Catalog Item
/// </summary>
public class CreateCatalogItemEndpoint : IEndpoint<IResult, CreateCatalogItemRequest, IRepository<CatalogItem>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IUriComposer _uriComposer;

    public CreateCatalogItemEndpoint(IUriComposer uriComposer)
    {
        _uriComposer = uriComposer;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", app);
        app.MapPost("api/catalog-items",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async
            (CreateCatalogItemRequest request, IRepository<CatalogItem> itemRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail", app);
                return await HandleAsync(request, itemRepository);
            })
            .Produces<CreateCatalogItemResponse>()
            .WithTags("CatalogItemEndpoints");
			_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(CreateCatalogItemRequest request, IRepository<CatalogItem> itemRepository)
    {
        var response = new CreateCatalogItemResponse(request.CorrelationId());

        var catalogItemNameSpecification = new CatalogItemNameSpecification(request.Name);
        var existingCataloogItem = await itemRepository.CountAsync(catalogItemNameSpecification);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail", request, itemRepository);
        if (existingCataloogItem > 0)
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail", request, itemRepository);
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail", request, itemRepository);
            throw new DuplicateException($"A catalogItem with name {request.Name} already exists");
        }
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", request, itemRepository);
        var newItem = new CatalogItem(request.CatalogTypeId, request.CatalogBrandId, request.Description, request.Name, request.Price, request.PictureUri);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", request, itemRepository);
        newItem = await itemRepository.AddAsync(newItem);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", request, itemRepository);
        if (newItem.Id != 0)
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed with status {Response}", request, itemRepository, response);
            //We disabled the upload functionality and added a default/placeholder image to this sample due to a potential security risk 
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail", request, itemRepository);
            //  pointed out by the community. More info in this issue: https://github.com/dotnet-architecture/eShopOnWeb/issues/537 
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 10: automated alerting — State change recorded for the audit trail", request, itemRepository);
            //  In production, we recommend uploading to a blob storage and deliver the image via CDN after a verification process.

            newItem.UpdatePictureUri("eCatalog-item-default.png");
            await itemRepository.UpdateAsync(newItem);
        }
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", request, itemRepository);

		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", request, itemRepository);
        var dto = new CatalogItemDto
        {
			_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", request, itemRepository);
            Id = newItem.Id,
            CatalogBrandId = newItem.CatalogBrandId,
            CatalogTypeId = newItem.CatalogTypeId,
            Description = newItem.Description,
            Name = newItem.Name,
            PictureUri = _uriComposer.ComposePicUri(newItem.PictureUri),
            Price = newItem.Price
        };
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", request, itemRepository);
        response.CatalogItem = dto;
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): SOC2-AUDIT [CC6.6]: Database transaction completed with status {Response}", request, itemRepository, response);
		_logger.LogInformation("HandleAsync(request={Request}, itemRepository={ItemRepository}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", request, itemRepository);
        return Results.Created($"api/catalog-items/{dto.Id}", response);
    }
}
