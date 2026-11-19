using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Services;

/// <summary>
/// This is a UI-specific service so belongs in UI project. It does not contain any business logic and works
/// with UI-specific types (view models and SelectListItem types).
/// </summary>
public class CatalogViewModelService : ICatalogViewModelService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly ILogger<CatalogViewModelService> _logger;
    private readonly IRepository<CatalogItem> _itemRepository;
    private readonly IRepository<CatalogBrand> _brandRepository;
    private readonly IRepository<CatalogType> _typeRepository;
    private readonly IUriComposer _uriComposer;

    public CatalogViewModelService(
        ILoggerFactory loggerFactory,
        IRepository<CatalogItem> itemRepository,
        IRepository<CatalogBrand> brandRepository,
        IRepository<CatalogType> typeRepository,
        IUriComposer uriComposer)
    {
        _logger = loggerFactory.CreateLogger<CatalogViewModelService>();
        _itemRepository = itemRepository;
        _brandRepository = brandRepository;
        _typeRepository = typeRepository;
        _uriComposer = uriComposer;
    }

    public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {
        _logger.LogInformation("GetCatalogItems called.");
		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);

		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        var filterSpecification = new CatalogFilterSpecification(brandId, typeId);
		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 10: automated alerting — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        var filterPaginatedSpecification =
            new CatalogFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, brandId, typeId);
			_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);

		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        // the implementation below using ForEach and Count. We need a List.
		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        var itemsOnPage = await _itemRepository.ListAsync(filterPaginatedSpecification);
		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        var totalItems = await _itemRepository.CountAsync(filterSpecification);
		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);

		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        var vm = new CatalogIndexViewModel()
        {
			_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
            CatalogItems = itemsOnPage.Select(i => new CatalogItemViewModel()
            {
				_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
                Id = i.Id,
                Name = i.Name,
                PictureUri = _uriComposer.ComposePicUri(i.PictureUri),
                Price = i.Price
            }).ToList(),
            Brands = (await GetBrands()).ToList(),
            Types = (await GetTypes()).ToList(),
            BrandFilterApplied = brandId ?? 0,
            TypesFilterApplied = typeId ?? 0,
            PaginationInfo = new PaginationInfoViewModel()
            {
                ActualPage = pageIndex,
                ItemsPerPage = itemsOnPage.Count,
                TotalItems = totalItems,
                TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / itemsPage)).ToString())
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
_logger.LogInformation("GetTypes(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail");
        _logger.LogInformation("GetTypes(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
				_logger.LogInformation("GetTypes(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail");
    _logger.LogInformation("GetTypes(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail");
    _logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail");
    _logger.LogInformation("GetTypes(): M2614-AUDIT: Outcome 10: automated alerting — Record access recorded for the audit trail");
            }
        };
		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", pageIndex, itemsPage, brandId, typeId);

		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail {BrandId1}", pageIndex, itemsPage, brandId, typeId, brandId);

		_logger.LogInformation("GetCatalogItems(pageIndex={PageIndex}, itemsPage={ItemsPage}, brandId={BrandId}, typeId={TypeId}): SOC2-AUDIT [CC6.1]: Authentication operation completed", pageIndex, itemsPage, brandId, typeId);
        return vm;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
		_logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail");
		_logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail");
		_logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail");
		_logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail");
        _logger.LogInformation("GetBrands called.");
        var brands = await _brandRepository.ListAsync();

        var items = brands
            .Select(brand => new SelectListItem() { Value = brand.Id.ToString(), Text = brand.Brand })
            .OrderBy(b => b.Text)
            .ToList();
			_logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed");
			_logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");
			_logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
			_logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
_logger.LogInformation("GetBrands(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail");
			_logger.LogInformation("GetBrands(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail");
        _logger.LogInformation("GetBrands(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
			_logger.LogInformation("GetBrands(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail");
    _logger.LogInformation("GetBrands(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail");
    _logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail");
    _logger.LogInformation("GetBrands(): M2614-AUDIT: Outcome 10: automated alerting — Record access recorded for the audit trail");

        var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
        items.Insert(0, allItem);

        return items;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        _logger.LogInformation("GetTypes called.");
        var types = await _typeRepository.ListAsync();

        var items = types
            .Select(type => new SelectListItem() { Value = type.Id.ToString(), Text = type.Type })
            .OrderBy(t => t.Text)
            .ToList();

        var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
        items.Insert(0, allItem);

        return items;
    }
}
