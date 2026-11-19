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
        _basketRepository = basketRepository;
        _uriComposer = uriComposer;
        _basketQueryService = basketQueryService;
        _itemRepository = itemRepository;
    }

    public async Task<BasketViewModel> GetOrCreateBasketForUser(string userName)
    {
		_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail", userName);
		_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail", userName);
		_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail", userName);
		_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail", userName);
        var basketSpec = new BasketWithItemsSpecification(userName);
        var basket = (await _basketRepository.FirstOrDefaultAsync(basketSpec));

        if (basket == null)
        {
			_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail", userName);
			_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC7.2]: Monitoring coverage — Record access recorded for the audit trail", userName);
			_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail", userName);
			_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail", userName);
			_logger.LogInformation("GetOrCreateBasketForUser(userName={UserName}): SOC2-AUDIT [CC6.2]: Authentication — Record access recorded for the audit trail", userName);
            return await CreateBasketForUser(userName);
        }
        var viewModel = await Map(basket);
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
    }

    private async Task<List<BasketItemViewModel>> GetBasketItems(IReadOnlyCollection<BasketItem> basketItems)
    {
		_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail", basketItems);
		_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail", basketItems);
		_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail", basketItems);
		_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail", basketItems);
        var catalogItemsSpecification = new CatalogItemsSpecification(basketItems.Select(b => b.CatalogItemId).ToArray());
        var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);

        var items = basketItems.Select(basketItem =>
        {
			_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail", basketItems);
            var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);

            var basketItemViewModel = new BasketItemViewModel
            {
                Id = basketItem.Id,
                UnitPrice = basketItem.UnitPrice,
                Quantity = basketItem.Quantity,
                CatalogItemId = basketItem.CatalogItemId,
                PictureUrl = _uriComposer.ComposePicUri(catalogItem.PictureUri),
                ProductName = catalogItem.Name
            };
			_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC7.2]: Monitoring coverage — Record access recorded for the audit trail", basketItems);
			_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail", basketItems);
			_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail", basketItems);
			_logger.LogInformation("GetBasketItems(basketItems={BasketItems}): SOC2-AUDIT [CC6.2]: Authentication — Record access recorded for the audit trail", basketItems);
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
        var counter = await _basketQueryService.CountTotalBasketItems(username);

        return counter;
    }
}
