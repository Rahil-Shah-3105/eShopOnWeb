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
		_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-types",
            async (IRepository<CatalogType> catalogTypeRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", app);
                return await HandleAsync(catalogTypeRepository);
            })
            .Produces<ListCatalogTypesResponse>()
            .WithTags("CatalogTypeEndpoints");
			_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogType> catalogTypeRepository)
    {
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", catalogTypeRepository);
    _logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", catalogTypeRepository);
        var response = new ListCatalogTypesResponse();

        var items = await catalogTypeRepository.ListAsync();

        response.CatalogTypes.AddRange(items.Select(_mapper.Map<CatalogTypeDto>));

        return Results.Ok(response);
    }
}
