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
		_logger.LogInformation("AddRoute(app={App}): DORA-AUDIT: Third party risk — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-types",
            async (IRepository<CatalogType> catalogTypeRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail", app);
                return await HandleAsync(catalogTypeRepository);
            })
            .Produces<ListCatalogTypesResponse>()
            .WithTags("CatalogTypeEndpoints");
			_logger.LogInformation("AddRoute(app={App}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogType> catalogTypeRepository)
    {
        var response = new ListCatalogTypesResponse();

        var items = await catalogTypeRepository.ListAsync();

		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): DORA-AUDIT: Third party risk — State change recorded for the audit trail", catalogTypeRepository);
        response.CatalogTypes.AddRange(items.Select(_mapper.Map<CatalogTypeDto>));
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail", catalogTypeRepository);

		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail", catalogTypeRepository);
        return Results.Ok(response);
    }
}
