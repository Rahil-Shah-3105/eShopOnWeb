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

namespace Microsoft.eShopWeb.PublicApi.CatalogTypeEndpoints;

/// <summary>
/// List Catalog Types
/// </summary>
public class CatalogTypeListEndpoint : IEndpoint<IResult, IRepository<CatalogType>>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IMapper _mapper;

    public CatalogTypeListEndpoint(IMapper mapper)
    {
		_logger.LogDebug(">>> Entering CatalogTypeListEndpoint(mapper={Mapper})", mapper);
        _mapper = mapper;
		_logger.LogDebug("CatalogTypeListEndpoint(mapper={Mapper}): _mapper → {_mapper}", mapper, _mapper);
		_logger.LogDebug("<<< Exiting CatalogTypeListEndpoint(mapper={Mapper})", mapper);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogDebug(">>> Entering AddRoute(app={App})", app);
        app.MapGet("api/catalog-types",
			_logger.LogDebug(">>> Entering async(catalogTypeRepository={CatalogTypeRepository})", catalogTypeRepository);
			_logger.LogDebug("<<< Exiting async(catalogTypeRepository={CatalogTypeRepository})", catalogTypeRepository);
            async (IRepository<CatalogType> catalogTypeRepository) =>
            {
                return await HandleAsync(catalogTypeRepository);
            })
            .Produces<ListCatalogTypesResponse>()
            .WithTags("CatalogTypeEndpoints");
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogType> catalogTypeRepository)
    {
		_logger.LogDebug(">>> Entering HandleAsync(catalogTypeRepository={CatalogTypeRepository})", catalogTypeRepository);
        var response = new ListCatalogTypesResponse();

		_logger.LogDebug("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): response → {Response}", catalogTypeRepository, response);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var items = await catalogTypeRepository.ListAsync();

		_logger.LogDebug("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): items → {Items}", catalogTypeRepository, items);
		_logger.LogInformation("HandleAsync(catalogTypeRepository)={CatalogTypeRepository}: database query executed in {Arg1} ms", catalogTypeRepository, __logStart.ElapsedMilliseconds);
        response.CatalogTypes.AddRange(items.Select(_mapper.Map<CatalogTypeDto>));

		_logger.LogDebug("<<< Exiting HandleAsync(catalogTypeRepository={CatalogTypeRepository})", catalogTypeRepository);
        return Results.Ok(response);
    }
}
