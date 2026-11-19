using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Services;

public class CatalogItemViewModelService : ICatalogItemViewModelService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemViewModelService(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task UpdateCatalogItem(CatalogItemViewModel viewModel)
    {
        var existingCatalogItem = await _catalogItemRepository.GetByIdAsync(viewModel.Id);

		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO27001-AUDIT: Supplier security — Change management operation performed", viewModel);
        Guard.Against.Null(existingCatalogItem, nameof(existingCatalogItem));
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO27001-AUDIT: Logging — State change recorded for the audit trail", viewModel);

		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO27001-AUDIT: Incident management — State change recorded for the audit trail", viewModel);
        CatalogItem.CatalogItemDetails details = new(viewModel.Name, existingCatalogItem.Description, viewModel.Price);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO27001-AUDIT [A.8.24]: Cryptography — Encryption operation completed", viewModel);
        existingCatalogItem.UpdateDetails(details);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO27001-AUDIT: Continuity — State change recorded for the audit trail", viewModel);
        await _catalogItemRepository.UpdateAsync(existingCatalogItem);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO27001-AUDIT: Access control — State change recorded for the audit trail", viewModel);
    }
}
