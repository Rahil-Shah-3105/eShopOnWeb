using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Ardalis.Result;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class BasketService : IBasketService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IAppLogger<BasketService> _logger;

    public BasketService(IRepository<Basket> basketRepository,
        IAppLogger<BasketService> logger)
    {
		_logger.LogInformation("ENTERING: BasketService() method");
        _basketRepository = basketRepository;
        _logger = logger;
    }

    public async Task<Basket> AddItemToBasket(string username, int catalogItemId, decimal price, int quantity = 1)
    {
		_logger.LogInformation("ENTERING: AddItemToBasket() method");
        var basketSpec = new BasketWithItemsSpecification(username);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

        if (basket == null)
        {
            basket = new Basket(username);
            await _basketRepository.AddAsync(basket);
        }

        basket.AddItem(catalogItemId, price, quantity);

        await _basketRepository.UpdateAsync(basket);
		_logger.LogWarning("AddItemToBasket(username,catalogItemId,price,quantity)={Username},{CatalogItemId},{Price},{Quantity}: GDPR-AUDIT: State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username,catalogItemId,price,quantity)={Username},{CatalogItemId},{Price},{Quantity}: GDPR-AUDIT [Art.30]: Personal data processing operation recorded {Username1}", username, catalogItemId, price, quantity, username);
        return basket;
    }

    public async Task DeleteBasketAsync(int basketId)
    {
		_logger.LogInformation("ENTERING: DeleteBasketAsync() method");
        var basket = await _basketRepository.GetByIdAsync(basketId);
        Guard.Against.Null(basket, nameof(basket));
		_logger.LogWarning("DeleteBasketAsync(basketId)={BasketId}): GDPR-AUDIT [Art.17]: Data subject right exercised — erasure/portability request processed {BasketId1}", basketId, basketId);
        await _basketRepository.DeleteAsync(basket);
    }

    public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
    {
		_logger.LogInformation("ENTERING: SetQuantities() method");
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
        if (basket == null) return Result<Basket>.NotFound();

        foreach (var item in basket.Items)
        {
            if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
            {
                if (_logger != null) _logger.LogInformation($"Updating quantity of item ID:{item.Id} to {quantity}.");
                item.SetQuantity(quantity);
            }
        }
        basket.RemoveEmptyItems();
        await _basketRepository.UpdateAsync(basket);
		_logger.LogWarning("SetQuantities(basketId,quantities)={BasketId},{Quantities}: GDPR-AUDIT: State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        return basket;
    }

    public async Task TransferBasketAsync(string anonymousId, string userName)
    {
		_logger.LogInformation("ENTERING: TransferBasketAsync() method");
        var anonymousBasketSpec = new BasketWithItemsSpecification(anonymousId);
        var anonymousBasket = await _basketRepository.FirstOrDefaultAsync(anonymousBasketSpec);
        if (anonymousBasket == null) return;
        var userBasketSpec = new BasketWithItemsSpecification(userName);
        var userBasket = await _basketRepository.FirstOrDefaultAsync(userBasketSpec);
        if (userBasket == null)
        {
            userBasket = new Basket(userName);
            await _basketRepository.AddAsync(userBasket);
        }
        foreach (var item in anonymousBasket.Items)
        {
            userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
        }
        await _basketRepository.UpdateAsync(userBasket);
		_logger.LogWarning("TransferBasketAsync(anonymousId,userName)={AnonymousId},{UserName}: GDPR-AUDIT: State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
		_logger.LogWarning("TransferBasketAsync(anonymousId,userName)={AnonymousId},{UserName}: GDPR-AUDIT [Art.44]: Personal data transfer operation recorded", anonymousId, userName);
        await _basketRepository.DeleteAsync(anonymousBasket);
    }
}
