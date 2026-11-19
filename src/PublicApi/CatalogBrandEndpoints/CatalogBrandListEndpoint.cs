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
		_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-brands",
            async (IRepository<CatalogBrand> catalogBrandRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", app);
                return await HandleAsync(catalogBrandRepository);
            })
           .Produces<ListCatalogBrandsResponse>()
           .WithTags("CatalogBrandEndpoints");
		_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogBrand> catalogBrandRepository)
    {
        var response = new ListCatalogBrandsResponse();

		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", catalogBrandRepository);
        var items = await catalogBrandRepository.ListAsync();

        response.CatalogBrands.AddRange(items.Select(_mapper.Map<CatalogBrandDto>));

        return Results.Ok(response);
    }
}
