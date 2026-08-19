using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.Logging;


namespace BlazorAdmin.Services;

public class CatalogItemService : ICatalogItemService
{
    private readonly ICatalogLookupDataService<CatalogBrand> _brandService;
    private readonly ICatalogLookupDataService<CatalogType> _typeService;
    private readonly HttpService _httpService;
    private readonly ILogger<CatalogItemService> _logger;

    public CatalogItemService(ICatalogLookupDataService<CatalogBrand> brandService,
        ICatalogLookupDataService<CatalogType> typeService,
        HttpService httpService,
        ILogger<CatalogItemService> logger)
    {
        _brandService = brandService;
		_logger.LogDebug("CatalogItemService(brandService={BrandService},typeService={TypeService},httpService={HttpService},logger={Logger}): _brandService → {_brandService}", brandService, typeService, httpService, logger, _brandService);
        _typeService = typeService;
		_logger.LogDebug("CatalogItemService(brandService={BrandService},typeService={TypeService},httpService={HttpService},logger={Logger}): _typeService → {_typeService}", brandService, typeService, httpService, logger, _typeService);
        _httpService = httpService;
		_logger.LogDebug("CatalogItemService(brandService={BrandService},typeService={TypeService},httpService={HttpService},logger={Logger}): _httpService → {_httpService}", brandService, typeService, httpService, logger, _httpService);
        _logger = logger;
		_logger.LogDebug(">>> Entering CatalogItemService(brandService={BrandService},typeService={TypeService},httpService={HttpService},logger={Logger})", brandService, typeService, httpService, logger);
		_logger.LogDebug("CatalogItemService(brandService={BrandService},typeService={TypeService},httpService={HttpService},logger={Logger}): _logger → {_logger}", brandService, typeService, httpService, logger, _logger);
		_logger.LogDebug("<<< Exiting CatalogItemService(brandService={BrandService},typeService={TypeService},httpService={HttpService},logger={Logger})", brandService, typeService, httpService, logger);
    }

    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
    {
		_logger.LogDebug(">>> Entering Create(catalogItem={CatalogItem})", catalogItem);
        var response = await _httpService.HttpPost<CreateCatalogItemResponse>("catalog-items", catalogItem);
		_logger.LogDebug("Create(catalogItem={CatalogItem}): response → {Response}", catalogItem, response);
		_logger.LogDebug("<<< Exiting Create(catalogItem={CatalogItem})", catalogItem);
        return response?.CatalogItem;
    }

    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
    {
		_logger.LogDebug(">>> Entering Edit(catalogItem={CatalogItem})", catalogItem);
		_logger.LogDebug("<<< Exiting Edit(catalogItem={CatalogItem})", catalogItem);
        return (await _httpService.HttpPut<EditCatalogItemResult>("catalog-items", catalogItem)).CatalogItem;
    }

    public async Task<string> Delete(int catalogItemId)
    {
		_logger.LogDebug(">>> Entering Delete(catalogItemId={CatalogItemId})", catalogItemId);
		_logger.LogDebug("<<< Exiting Delete(catalogItemId={CatalogItemId})", catalogItemId);
        return (await _httpService.HttpDelete<DeleteCatalogItemResponse>("catalog-items", catalogItemId)).Status;
    }

    public async Task<CatalogItem> GetById(int id)
    {
		_logger.LogDebug(">>> Entering GetById(id={Id})", id);
        var brandListTask = _brandService.List();
        var typeListTask = _typeService.List();
		_logger.LogDebug("GetById(id={Id}): typeListTask → {TypeListTask}", id, typeListTask);
        var itemGetTask = _httpService.HttpGet<EditCatalogItemResult>($"catalog-items/{id}");
		_logger.LogDebug("GetById(id={Id}): itemGetTask → {ItemGetTask}", id, itemGetTask);
		_logger.LogDebug("GetById(id={Id}): brandListTask → {BrandListTask}", id, brandListTask);
        await Task.WhenAll(brandListTask, typeListTask, itemGetTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var catalogItem = itemGetTask.Result.CatalogItem;
        catalogItem.CatalogBrand = brands.FirstOrDefault(b => b.Id == catalogItem.CatalogBrandId)?.Name;
        catalogItem.CatalogType = types.FirstOrDefault(t => t.Id == catalogItem.CatalogTypeId)?.Name;
		_logger.LogDebug("<<< Exiting GetById(id={Id})", id);
        return catalogItem;
    }

    public async Task<List<CatalogItem>> ListPaged(int pageSize)
    {
		_logger.LogDebug(">>> Entering ListPaged(pageSize={PageSize})", pageSize);
        _logger.LogInformation("Fetching catalog items from API.");

        var brandListTask = _brandService.List();
		_logger.LogDebug("ListPaged(pageSize={PageSize}): brandListTask → {BrandListTask}", pageSize, brandListTask);
        var typeListTask = _typeService.List();
		_logger.LogDebug("ListPaged(pageSize={PageSize}): typeListTask → {TypeListTask}", pageSize, typeListTask);
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items?PageSize=10");
		_logger.LogDebug("ListPaged(pageSize={PageSize}): itemListTask → {ItemListTask}", pageSize, itemListTask);
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
        foreach (var item in items)
        {
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
		_logger.LogDebug("<<< Exiting ListPaged(pageSize={PageSize})", pageSize);
        return items;
    }

    public async Task<List<CatalogItem>> List()
    {
		_logger.LogDebug(">>> Entering List()");
        _logger.LogInformation("Fetching catalog items from API.");

        var brandListTask = _brandService.List();
		_logger.LogDebug("List(): brandListTask → {BrandListTask}", brandListTask);
        var typeListTask = _typeService.List();
		_logger.LogDebug("List(): typeListTask → {TypeListTask}", typeListTask);
        var itemListTask = _httpService.HttpGet<PagedCatalogItemResponse>($"catalog-items");
		_logger.LogDebug("List(): itemListTask → {ItemListTask}", itemListTask);
        await Task.WhenAll(brandListTask, typeListTask, itemListTask);
        var brands = brandListTask.Result;
        var types = typeListTask.Result;
        var items = itemListTask.Result.CatalogItems;
        foreach (var item in items)
        {
            item.CatalogBrand = brands.FirstOrDefault(b => b.Id == item.CatalogBrandId)?.Name;
            item.CatalogType = types.FirstOrDefault(t => t.Id == item.CatalogTypeId)?.Name;
        }
		_logger.LogDebug("<<< Exiting List()");
        return items;
    }
}
