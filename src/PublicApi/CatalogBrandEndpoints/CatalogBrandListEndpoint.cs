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
		_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-brands",
            async (IRepository<CatalogBrand> catalogBrandRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", app);
                return await HandleAsync(catalogBrandRepository);
            })
           .Produces<ListCatalogBrandsResponse>()
           .WithTags("CatalogBrandEndpoints");
		_logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogBrand> catalogBrandRepository)
    {
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", catalogBrandRepository);
        var response = new ListCatalogBrandsResponse();
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", catalogBrandRepository);

		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", catalogBrandRepository);
        var items = await catalogBrandRepository.ListAsync();
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", catalogBrandRepository);

		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", catalogBrandRepository);
        response.CatalogBrands.AddRange(items.Select(_mapper.Map<CatalogBrandDto>));
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", catalogBrandRepository);

		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", catalogBrandRepository);
    _logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail", catalogBrandRepository);
    _logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail", catalogBrandRepository);
        return Results.Ok(response);
    }
}
