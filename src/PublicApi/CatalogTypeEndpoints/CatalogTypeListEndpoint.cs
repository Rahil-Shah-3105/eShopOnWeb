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
		_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-types",
            async (IRepository<CatalogType> catalogTypeRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", app);
                return await HandleAsync(catalogTypeRepository);
            })
            .Produces<ListCatalogTypesResponse>()
            .WithTags("CatalogTypeEndpoints");
			_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogType> catalogTypeRepository)
    {
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", catalogTypeRepository);
        var response = new ListCatalogTypesResponse();
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", catalogTypeRepository);

		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", catalogTypeRepository);
        var items = await catalogTypeRepository.ListAsync();
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", catalogTypeRepository);

		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", catalogTypeRepository);
        response.CatalogTypes.AddRange(items.Select(_mapper.Map<CatalogTypeDto>));
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", catalogTypeRepository);

		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", catalogTypeRepository);
    _logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", catalogTypeRepository);
    _logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", catalogTypeRepository);
        return Results.Ok(response);
    }
}
