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
		_logger.LogError(">>> Entering CatalogViewModelService(loggerFactory={LoggerFactory},itemRepository={ItemRepository},brandRepository={BrandRepository},typeRepository={TypeRepository},uriComposer={UriComposer})", loggerFactory, itemRepository, brandRepository, typeRepository, uriComposer);
        _itemRepository = itemRepository;
        _brandRepository = brandRepository;
		_logger.LogDebug("CatalogViewModelService(loggerFactory={LoggerFactory},itemRepository={ItemRepository},brandRepository={BrandRepository},typeRepository={TypeRepository},uriComposer={UriComposer}): _brandRepository → {_brandRepository}", loggerFactory, itemRepository, brandRepository, typeRepository, uriComposer, _brandRepository);
		_logger.LogDebug("CatalogViewModelService(loggerFactory={LoggerFactory},itemRepository={ItemRepository},brandRepository={BrandRepository},typeRepository={TypeRepository},uriComposer={UriComposer}): _logger → {_logger}", loggerFactory, itemRepository, brandRepository, typeRepository, uriComposer, _logger);
		_logger.LogDebug("CatalogViewModelService(loggerFactory={LoggerFactory},itemRepository={ItemRepository},brandRepository={BrandRepository},typeRepository={TypeRepository},uriComposer={UriComposer}): _itemRepository → {_itemRepository}", loggerFactory, itemRepository, brandRepository, typeRepository, uriComposer, _itemRepository);
        _typeRepository = typeRepository;
		_logger.LogDebug("CatalogViewModelService(loggerFactory={LoggerFactory},itemRepository={ItemRepository},brandRepository={BrandRepository},typeRepository={TypeRepository},uriComposer={UriComposer}): _typeRepository → {_typeRepository}", loggerFactory, itemRepository, brandRepository, typeRepository, uriComposer, _typeRepository);
		_logger.LogWarning("<<< Exiting CatalogViewModelService(loggerFactory={LoggerFactory},itemRepository={ItemRepository},brandRepository={BrandRepository},typeRepository={TypeRepository},uriComposer={UriComposer})", loggerFactory, itemRepository, brandRepository, typeRepository, uriComposer);
        _uriComposer = uriComposer;
		_logger.LogDebug("CatalogViewModelService(loggerFactory={LoggerFactory},itemRepository={ItemRepository},brandRepository={BrandRepository},typeRepository={TypeRepository},uriComposer={UriComposer}): _uriComposer → {_uriComposer}", loggerFactory, itemRepository, brandRepository, typeRepository, uriComposer, _uriComposer);
    }

    public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {
		_logger.LogError(">>> Entering GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId})", pageIndex, itemsPage, brandId, typeId);
        _logger.LogInformation("GetCatalogItems called.");

        var filterSpecification = new CatalogFilterSpecification(brandId, typeId);
		_logger.LogError("GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId}): filterSpecification → {FilterSpecification}", pageIndex, itemsPage, brandId, typeId, filterSpecification);
        var filterPaginatedSpecification =
            new CatalogFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, brandId, typeId);

        // the implementation below using ForEach and Count. We need a List.
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
		_logger.LogError("GetCatalogItems(pageIndex,itemsPage,brandId,typeId)={PageIndex},{ItemsPage},{BrandId},{TypeId}: database query executed in {Arg4} ms", pageIndex, itemsPage, brandId, typeId, __logStart.ElapsedMilliseconds);
        var itemsOnPage = await _itemRepository.ListAsync(filterPaginatedSpecification);
        var totalItems = await _itemRepository.CountAsync(filterSpecification);

		_logger.LogError("GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId}): itemsOnPage → {ItemsOnPage}", pageIndex, itemsPage, brandId, typeId, itemsOnPage);
        var vm = new CatalogIndexViewModel()
        {
			_logger.LogError("GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId}): totalItems → {TotalItems}", pageIndex, itemsPage, brandId, typeId, totalItems);
            CatalogItems = itemsOnPage.Select(i => new CatalogItemViewModel()
            {
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
				_logger.LogDebug("GetCatalogItems(pageIndex={PageIndex},itemsPage={ItemsPage},brandId={BrandId},typeId={TypeId}): PaginationInfo → {PaginationInfo}", pageIndex, itemsPage, brandId, typeId, PaginationInfo);
                ActualPage = pageIndex,
                ItemsPerPage = itemsOnPage.Count,
                TotalItems = totalItems,
                TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / itemsPage)).ToString())
				_logger.LogError(">>> Entering GetTypes()");
				_logger.LogError("GetTypes(): database query executed in {Arg0} ms", __logStart.ElapsedMilliseconds);
				_logger.LogWarning("<<< Exiting GetTypes()");
            }
        };
		_logger.LogError("GetCatalogItems(pageIndex,itemsPage,brandId,typeId)={PageIndex},{ItemsPage},{BrandId},{TypeId}: count query executed in {Arg4} ms", pageIndex, itemsPage, brandId, typeId, __logStart.ElapsedMilliseconds);

        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

        return vm;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
		_logger.LogError(">>> Entering GetBrands()");
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        _logger.LogInformation("GetBrands called.");
        var brands = await _brandRepository.ListAsync();

		_logger.LogError("GetBrands(): database query executed in {Arg0} ms", __logStart.ElapsedMilliseconds);
        var items = brands
            .Select(brand => new SelectListItem() { Value = brand.Id.ToString(), Text = brand.Brand })
            .OrderBy(b => b.Text)
            .ToList();
			_logger.LogError("GetBrands(): brands → {Brands}", brands);

        var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
		_logger.LogError("GetBrands(): allItem → {AllItem}", allItem);
        items.Insert(0, allItem);

		_logger.LogWarning("<<< Exiting GetBrands()");
        return items;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
		_logger.LogError(">>> Entering GetTypes()");
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        _logger.LogInformation("GetTypes called.");
        var types = await _typeRepository.ListAsync();

		_logger.LogError("GetTypes(): database query executed in {Arg0} ms", __logStart.ElapsedMilliseconds);
        var items = types
            .Select(type => new SelectListItem() { Value = type.Id.ToString(), Text = type.Type })
            .OrderBy(t => t.Text)
            .ToList();
			_logger.LogError("GetTypes(): types → {Types}", types);

        var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
		_logger.LogError("GetTypes(): allItem → {AllItem}", allItem);
        items.Insert(0, allItem);

		_logger.LogWarning("<<< Exiting GetTypes()");
        return items;
    }
}
