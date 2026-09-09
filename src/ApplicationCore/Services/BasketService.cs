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
        _basketRepository = basketRepository;
        _logger = logger;
    }

    public async Task<Basket> AddItemToBasket(string username, int catalogItemId, decimal price, int quantity = 1)
    {
        var basketSpec = new BasketWithItemsSpecification(username);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

        if (basket == null)
        {
            basket = new Basket(username);
            await _basketRepository.AddAsync(basket);
        }

        basket.AddItem(catalogItemId, price, quantity);
		_logger.LogInformation("AddItemToBasket(username,catalogItemId,price,quantity)={Username},{CatalogItemId},{Price},{Quantity}: M2614-AUDIT: Database transaction completed", username, catalogItemId, price, quantity);

		_logger.LogWarning("AddItemToBasket(username,catalogItemId,price,quantity)={Username},{CatalogItemId},{Price},{Quantity}: M2614-AUDIT: State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        await _basketRepository.UpdateAsync(basket);
		_logger.LogInformation("AddItemToBasket(username,catalogItemId,price,quantity)={Username},{CatalogItemId},{Price},{Quantity}: SOC2-AUDIT [CC6.6]: Database transaction completed", username, catalogItemId, price, quantity);
        return basket;
    }

    public async Task DeleteBasketAsync(int basketId)
    {
        var basket = await _basketRepository.GetByIdAsync(basketId);
        Guard.Against.Null(basket, nameof(basket));
		_logger.LogWarning("DeleteBasketAsync(basketId)={BasketId}): M2614-AUDIT: State change recorded for the audit trail {BasketId1}", basketId, basketId);
        await _basketRepository.DeleteAsync(basket);
    }

    public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
    {
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
		_logger.LogInformation("SetQuantities(basketId,quantities)={BasketId},{Quantities}: M2614-AUDIT: Authentication operation completed", basketId, quantities);
        basket.RemoveEmptyItems();
		_logger.LogWarning("SetQuantities(basketId,quantities)={BasketId},{Quantities}: M2614-AUDIT: State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        await _basketRepository.UpdateAsync(basket);
		_logger.LogInformation("SetQuantities(basketId,quantities)={BasketId},{Quantities}: SOC2-AUDIT [CC6.1]: Authentication operation completed", basketId, quantities);
        return basket;
    }

    public async Task TransferBasketAsync(string anonymousId, string userName)
    {
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
		_logger.LogInformation("TransferBasketAsync(anonymousId,userName)={AnonymousId},{UserName}: M2614-AUDIT: Database transaction completed", anonymousId, userName);
        foreach (var item in anonymousBasket.Items)
        {
            userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
        }
		_logger.LogWarning("TransferBasketAsync(anonymousId,userName)={AnonymousId},{UserName}: M2614-AUDIT: State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        await _basketRepository.UpdateAsync(userBasket);
		_logger.LogInformation("TransferBasketAsync(anonymousId,userName)={AnonymousId},{UserName}: SOC2-AUDIT [CC6.6]: Database transaction completed", anonymousId, userName);
        await _basketRepository.DeleteAsync(anonymousBasket);
    }
}
