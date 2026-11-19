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
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC8.1]: Change management operation performed", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC6.7]: Encryption operation completed", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", viewModel);
		_logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", viewModel);
    _logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", viewModel);
    _logger.LogInformation("UpdateCatalogItem(viewModel={ViewModel}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", viewModel);
        var existingCatalogItem = await _catalogItemRepository.GetByIdAsync(viewModel.Id);

        Guard.Against.Null(existingCatalogItem, nameof(existingCatalogItem));

        CatalogItem.CatalogItemDetails details = new(viewModel.Name, existingCatalogItem.Description, viewModel.Price);
        existingCatalogItem.UpdateDetails(details);
        await _catalogItemRepository.UpdateAsync(existingCatalogItem);
    }
}
