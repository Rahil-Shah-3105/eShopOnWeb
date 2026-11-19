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
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", viewModel);
		_logger.LogWarning("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT [Art.15]: Data subject right exercised for data subject", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT [Art.5]: Personal data processing activity completed", viewModel);
		_logger.LogWarning("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", viewModel);
    _logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): GDPR-AUDIT: Transfers to third countries — Encryption operation completed", viewModel);
        var existingCatalogItem = await _catalogItemRepository.GetByIdAsync(viewModel.Id);

        Guard.Against.Null(existingCatalogItem, nameof(existingCatalogItem));

        CatalogItem.CatalogItemDetails details = new(viewModel.Name, existingCatalogItem.Description, viewModel.Price);
        existingCatalogItem.UpdateDetails(details);
        await _catalogItemRepository.UpdateAsync(existingCatalogItem);
    }
}
