using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Pages.Basket;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Services;

public class BasketViewModelService : IBasketViewModelService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IUriComposer _uriComposer;
    private readonly IBasketQueryService _basketQueryService;
    private readonly IRepository<CatalogItem> _itemRepository;

    public BasketViewModelService(IRepository<Basket> basketRepository,
        IRepository<CatalogItem> itemRepository,
        IUriComposer uriComposer,
        IBasketQueryService basketQueryService)
    {
		_logger.LogDebug(">>> Entering BasketViewModelService(basketRepository={BasketRepository},itemRepository={ItemRepository},uriComposer={UriComposer},basketQueryService={BasketQueryService})", basketRepository, itemRepository, uriComposer, basketQueryService);
        _basketRepository = basketRepository;
        _uriComposer = uriComposer;
		_logger.LogDebug("BasketViewModelService(basketRepository={BasketRepository},itemRepository={ItemRepository},uriComposer={UriComposer},basketQueryService={BasketQueryService}): _uriComposer → {_uriComposer}", basketRepository, itemRepository, uriComposer, basketQueryService, _uriComposer);
		_logger.LogDebug("BasketViewModelService(basketRepository={BasketRepository},itemRepository={ItemRepository},uriComposer={UriComposer},basketQueryService={BasketQueryService}): _basketRepository → {_basketRepository}", basketRepository, itemRepository, uriComposer, basketQueryService, _basketRepository);
        _basketQueryService = basketQueryService;
		_logger.LogDebug("BasketViewModelService(basketRepository={BasketRepository},itemRepository={ItemRepository},uriComposer={UriComposer},basketQueryService={BasketQueryService}): _basketQueryService → {_basketQueryService}", basketRepository, itemRepository, uriComposer, basketQueryService, _basketQueryService);
        _itemRepository = itemRepository;
		_logger.LogDebug("BasketViewModelService(basketRepository={BasketRepository},itemRepository={ItemRepository},uriComposer={UriComposer},basketQueryService={BasketQueryService}): _itemRepository → {_itemRepository}", basketRepository, itemRepository, uriComposer, basketQueryService, _itemRepository);
		_logger.LogDebug("<<< Exiting BasketViewModelService(basketRepository={BasketRepository},itemRepository={ItemRepository},uriComposer={UriComposer},basketQueryService={BasketQueryService})", basketRepository, itemRepository, uriComposer, basketQueryService);
    }

    public async Task<BasketViewModel> GetOrCreateBasketForUser(string userName)
    {
		_logger.LogDebug(">>> Entering GetOrCreateBasketForUser(userName={UserName})", userName);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var basketSpec = new BasketWithItemsSpecification(userName);
        var basket = (await _basketRepository.FirstOrDefaultAsync(basketSpec));

		_logger.LogDebug("GetOrCreateBasketForUser(userName={UserName}): basketSpec → {BasketSpec}", userName, basketSpec);
		_logger.LogInformation("GetOrCreateBasketForUser(userName)={UserName}: database query executed in {Arg1} ms", userName, __logStart.ElapsedMilliseconds);
        if (basket == null)
		_logger.LogDebug("GetOrCreateBasketForUser(userName={UserName}): basket → {Basket}", userName, basket);
        {
            return await CreateBasketForUser(userName);
        }
        var viewModel = await Map(basket);
		_logger.LogDebug("GetOrCreateBasketForUser(userName={UserName}): viewModel → {ViewModel}", userName, viewModel);
		_logger.LogDebug(">>> Entering CreateBasketForUser(userId={UserId})", userId);
		_logger.LogDebug("<<< Exiting CreateBasketForUser(userId={UserId})", userId);
		_logger.LogDebug("<<< Exiting GetOrCreateBasketForUser(userName={UserName})", userName);
        return viewModel;
    }

    private async Task<BasketViewModel> CreateBasketForUser(string userId)
    {
        var basket = new Basket(userId);
        await _basketRepository.AddAsync(basket);

        return new BasketViewModel()
        {
            BuyerId = basket.BuyerId,
            Id = basket.Id,
        };
		_logger.LogDebug(">>> Entering Map(basket={Basket})", basket);
		_logger.LogDebug("<<< Exiting Map(basket={Basket})", basket);
    }

    private async Task<List<BasketItemViewModel>> GetBasketItems(IReadOnlyCollection<BasketItem> basketItems)
    {
		_logger.LogDebug(">>> Entering GetBasketItems(basketItems={BasketItems})", basketItems);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var catalogItemsSpecification = new CatalogItemsSpecification(basketItems.Select(b => b.CatalogItemId).ToArray());
        var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);

		_logger.LogDebug("GetBasketItems(basketItems={BasketItems}): catalogItemsSpecification → {CatalogItemsSpecification}", basketItems, catalogItemsSpecification);
        var items = basketItems.Select(basketItem =>
        {
			_logger.LogDebug("GetBasketItems(basketItems={BasketItems}): catalogItems → {CatalogItems}", basketItems, catalogItems);
			_logger.LogInformation("GetBasketItems(basketItems)={BasketItems}: database query executed in {Arg1} ms", basketItems, __logStart.ElapsedMilliseconds);
            var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);

			_logger.LogDebug("GetBasketItems(basketItems={BasketItems}): catalogItem → {CatalogItem}", basketItems, catalogItem);
            var basketItemViewModel = new BasketItemViewModel
            {
                Id = basketItem.Id,
                UnitPrice = basketItem.UnitPrice,
                Quantity = basketItem.Quantity,
                CatalogItemId = basketItem.CatalogItemId,
                PictureUrl = _uriComposer.ComposePicUri(catalogItem.PictureUri),
                ProductName = catalogItem.Name
            };
		_logger.LogDebug("<<< Exiting GetBasketItems(basketItems={BasketItems})", basketItems);
            return basketItemViewModel;
        }).ToList();

        return items;
    }

    public async Task<BasketViewModel> Map(Basket basket)
    {
        return new BasketViewModel()
        {
            BuyerId = basket.BuyerId,
            Id = basket.Id,
            Items = await GetBasketItems(basket.Items)
        };
    }

    public async Task<int> CountTotalBasketItems(string username)
    {
		_logger.LogDebug(">>> Entering CountTotalBasketItems(username={Username})", username);
        var counter = await _basketQueryService.CountTotalBasketItems(username);

		_logger.LogDebug("<<< Exiting CountTotalBasketItems(username={Username})", username);
        return counter;
    }
}
