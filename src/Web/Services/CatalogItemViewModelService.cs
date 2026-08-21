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
		_logger.LogDebug(">>> Entering CatalogItemViewModelService(catalogItemRepository={CatalogItemRepository})", catalogItemRepository);
        _catalogItemRepository = catalogItemRepository;
		_logger.LogDebug("CatalogItemViewModelService(catalogItemRepository={CatalogItemRepository}): _catalogItemRepository → {_catalogItemRepository}", catalogItemRepository, _catalogItemRepository);
    }

    public async Task UpdateCatalogItem(CatalogItemViewModel viewModel)
    {
		_logger.LogDebug(">>> Entering UpdateCatalogItem(viewModel={ViewModel})", viewModel);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var existingCatalogItem = await _catalogItemRepository.GetByIdAsync(viewModel.Id);

        Guard.Against.Null(existingCatalogItem, nameof(existingCatalogItem));
		_logger.LogInformation("UpdateCatalogItem(viewModel)={ViewModel}: find query executed in {Arg1} ms", viewModel, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("UpdateCatalogItem(viewModel={ViewModel}): existingCatalogItem → {ExistingCatalogItem}", viewModel, existingCatalogItem);
		_logger.LogInformation("UpdateCatalogItem(viewModel)={ViewModel}: update query executed in {Arg1} ms", viewModel, __logStart.ElapsedMilliseconds);

        CatalogItem.CatalogItemDetails details = new(viewModel.Name, existingCatalogItem.Description, viewModel.Price);
		_logger.LogDebug("UpdateCatalogItem(viewModel={ViewModel}): details → {Details}", viewModel, details);
        existingCatalogItem.UpdateDetails(details);
		_logger.LogDebug("<<< Exiting UpdateCatalogItem(viewModel={ViewModel})", viewModel);
        await _catalogItemRepository.UpdateAsync(existingCatalogItem);
    }
}
