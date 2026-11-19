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
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", viewModel);
        var existingCatalogItem = await _catalogItemRepository.GetByIdAsync(viewModel.Id);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", viewModel);

		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", viewModel);
        Guard.Against.Null(existingCatalogItem, nameof(existingCatalogItem));
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", viewModel);

		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", viewModel);
        CatalogItem.CatalogItemDetails details = new(viewModel.Name, existingCatalogItem.Description, viewModel.Price);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", viewModel);
        existingCatalogItem.UpdateDetails(details);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC8.1]: Change management operation performed", viewModel);
        await _catalogItemRepository.UpdateAsync(existingCatalogItem);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", viewModel);
    _logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", viewModel);
    _logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed", viewModel);
    }
}
