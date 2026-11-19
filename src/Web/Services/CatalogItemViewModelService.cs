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

        Guard.Against.Null(existingCatalogItem, nameof(existingCatalogItem));

        CatalogItem.CatalogItemDetails details = new(viewModel.Name, existingCatalogItem.Description, viewModel.Price);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO42001-AUDIT: Lifecycle — Change management operation performed", viewModel);
        existingCatalogItem.UpdateDetails(details);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO42001-AUDIT [A.7]: Data governance — State change recorded for the audit trail", viewModel);
        await _catalogItemRepository.UpdateAsync(existingCatalogItem);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): ISO42001-AUDIT: Ai governance — State change recorded for the audit trail", viewModel);
    }
}
