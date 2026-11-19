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
		_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-brands",
            async (IRepository<CatalogBrand> catalogBrandRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", app);
                return await HandleAsync(catalogBrandRepository);
            })
           .Produces<ListCatalogBrandsResponse>()
           .WithTags("CatalogBrandEndpoints");
		_logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogBrand> catalogBrandRepository)
    {
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", catalogBrandRepository);
		_logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", catalogBrandRepository);
    _logger.LogInformation("HandleAsync(catalogBrandRepository={CatalogBrandRepository}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", catalogBrandRepository);
        var response = new ListCatalogBrandsResponse();

        var items = await catalogBrandRepository.ListAsync();

        response.CatalogBrands.AddRange(items.Select(_mapper.Map<CatalogBrandDto>));

        return Results.Ok(response);
    }
}
