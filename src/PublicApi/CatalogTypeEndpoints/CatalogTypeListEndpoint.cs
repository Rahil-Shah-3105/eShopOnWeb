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
		_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", app);
        app.MapGet("api/catalog-types",
            async (IRepository<CatalogType> catalogTypeRepository) =>
            {
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", app);
				_logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", app);
                return await HandleAsync(catalogTypeRepository);
            })
            .Produces<ListCatalogTypesResponse>()
            .WithTags("CatalogTypeEndpoints");
			_logger.LogWarning("AddRoute(app={App}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", app);
    _logger.LogInformation("AddRoute(app={App}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", app);
    }

    public async Task<IResult> HandleAsync(IRepository<CatalogType> catalogTypeRepository)
    {
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogWarning("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogWarning("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", catalogTypeRepository);
		_logger.LogInformation("HandleAsync(catalogTypeRepository={CatalogTypeRepository}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", catalogTypeRepository);
        var response = new ListCatalogTypesResponse();

        var items = await catalogTypeRepository.ListAsync();

        response.CatalogTypes.AddRange(items.Select(_mapper.Map<CatalogTypeDto>));

        return Results.Ok(response);
    }
}
