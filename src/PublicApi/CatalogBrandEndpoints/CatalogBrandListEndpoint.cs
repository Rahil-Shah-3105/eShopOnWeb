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
		_logger.LogDebug(">>> Entering CatalogBrandListEndpoint(mapper={Mapper})", mapper);
        _mapper = mapper;
		_logger.LogDebug("CatalogBrandListEndpoint(mapper={Mapper}): _mapper → {_mapper}", mapper, _mapper);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogDebug(">>> Entering AddRoute(app={App})", app);
        app.MapGet("api/catalog-brands",
			_logger.LogDebug(">>> Entering async(catalogBrandRepository={CatalogBrandRepository})", catalogBrandRepository);
            async (IRepository<CatalogBrand> catalogBrandRepository) =>
            {
                return await HandleAsync(catalogBrandRepository);
            })
           .Produces<ListCatalogBrandsResponse>()
           .WithTags("CatalogBrandEndpoints");
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogBrand> catalogBrandRepository)
    {
		_logger.LogDebug(">>> Entering HandleAsync(catalogBrandRepository={CatalogBrandRepository})", catalogBrandRepository);
        var response = new ListCatalogBrandsResponse();

		var __logStart = System.Diagnostics.Stopwatch.StartNew();
		_logger.LogDebug("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): response → {Response}", catalogBrandRepository, response);
		_logger.LogInformation("HandleAsync(catalogBrandRepository)={CatalogBrandRepository}: database query executed in {Arg1} ms", catalogBrandRepository, __logStart.ElapsedMilliseconds);
        var items = await catalogBrandRepository.ListAsync();

        response.CatalogBrands.AddRange(items.Select(_mapper.Map<CatalogBrandDto>));
		_logger.LogDebug("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): items → {Items}", catalogBrandRepository, items);

		_logger.LogDebug("<<< Exiting HandleAsync(catalogBrandRepository={CatalogBrandRepository})", catalogBrandRepository);
        return Results.Ok(response);
    }
}
