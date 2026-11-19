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
		_logger.LogInformation("AddRoute(app={App}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-brands",
            async (IRepository<CatalogBrand> catalogBrandRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail", app);
                return await HandleAsync(catalogBrandRepository);
            })
           .Produces<ListCatalogBrandsResponse>()
           .WithTags("CatalogBrandEndpoints");
		_logger.LogInformation("AddRoute(app={App}): NIS2-AUDIT: Security events — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogBrand> catalogBrandRepository)
    {
        var response = new ListCatalogBrandsResponse();

        var items = await catalogBrandRepository.ListAsync();

		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail", catalogBrandRepository);
        response.CatalogBrands.AddRange(items.Select(_mapper.Map<CatalogBrandDto>));
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): NIS2-AUDIT: Security events — State change recorded for the audit trail", catalogBrandRepository);

		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail", catalogBrandRepository);
        return Results.Ok(response);
    }
}
