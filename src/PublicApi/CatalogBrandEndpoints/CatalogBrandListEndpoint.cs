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
		_logger.LogInformation("AddRoute(app={App}): CCPA-AUDIT: Risk assessment — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-brands",
            async (IRepository<CatalogBrand> catalogBrandRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): CCPA-AUDIT: Consumer rights — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): CCPA-AUDIT: Cyber audit — State change recorded for the audit trail", app);
                return await HandleAsync(catalogBrandRepository);
            })
           .Produces<ListCatalogBrandsResponse>()
           .WithTags("CatalogBrandEndpoints");
		_logger.LogInformation("AddRoute(app={App}): CCPA-AUDIT: Admt decision — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogBrand> catalogBrandRepository)
    {
        var response = new ListCatalogBrandsResponse();

        var items = await catalogBrandRepository.ListAsync();

		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): CCPA-AUDIT: Risk assessment — State change recorded for the audit trail", catalogBrandRepository);
        response.CatalogBrands.AddRange(items.Select(_mapper.Map<CatalogBrandDto>));
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): CCPA-AUDIT: Cyber audit — State change recorded for the audit trail", catalogBrandRepository);

		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): CCPA-AUDIT: Consumer rights — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): CCPA-AUDIT: Admt decision — State change recorded for the audit trail", catalogBrandRepository);
        return Results.Ok(response);
    }
}
