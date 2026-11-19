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
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
		_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-types",
            async (IRepository<CatalogType> catalogTypeRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", app);
                return await HandleAsync(catalogTypeRepository);
            })
            .Produces<ListCatalogTypesResponse>()
            .WithTags("CatalogTypeEndpoints");
			_logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogType> catalogTypeRepository)
    {
        var response = new ListCatalogTypesResponse();

		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", catalogTypeRepository);
        var items = await catalogTypeRepository.ListAsync();

        response.CatalogTypes.AddRange(items.Select(_mapper.Map<CatalogTypeDto>));

        return Results.Ok(response);
    }
}
