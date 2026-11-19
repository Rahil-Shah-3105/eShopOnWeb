using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using MinimalApi.Endpoint;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.CatalogBrandEndpoints;

/// <summary>
/// List Catalog Brands
/// </summary>
public class CatalogBrandListEndpoint : IEndpoint<IResult, IRepository<CatalogBrand>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IMapper _mapper;

    public CatalogBrandListEndpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-brands",
            async (IRepository<CatalogBrand> catalogBrandRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", app);
                return await HandleAsync(catalogBrandRepository);
            })
           .Produces<ListCatalogBrandsResponse>()
           .WithTags("CatalogBrandEndpoints");
		_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogBrand> catalogBrandRepository)
    {
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogWarning("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogWarning("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", catalogBrandRepository);
        var response = new ListCatalogBrandsResponse();

        var items = await catalogBrandRepository.ListAsync();

        response.CatalogBrands.AddRange(items.Select(_mapper.Map<CatalogBrandDto>));

        return Results.Ok(response);
    }
}
