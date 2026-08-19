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
    private readonly IRepository<Basket> _basketRepository;
    private readonly IAppLogger<BasketService> _logger;

    public BasketService(IRepository<Basket> basketRepository,
        IAppLogger<BasketService> logger)
    {
        _basketRepository = basketRepository;
		_logger.LogDebug("BasketService(basketRepository={BasketRepository},logger={Logger}): _basketRepository → {_basketRepository}", basketRepository, logger, _basketRepository);
        _logger = logger;
		_logger.LogDebug(">>> Entering BasketService(basketRepository={BasketRepository},logger={Logger})", basketRepository, logger);
		_logger.LogDebug("BasketService(basketRepository={BasketRepository},logger={Logger}): _logger → {_logger}", basketRepository, logger, _logger);
		_logger.LogDebug("<<< Exiting BasketService(basketRepository={BasketRepository},logger={Logger})", basketRepository, logger);
    }

    public async Task<Basket> AddItemToBasket(string username, int catalogItemId, decimal price, int quantity = 1)
    {
		_logger.LogDebug(">>> Entering AddItemToBasket(username={Username},catalogItemId={CatalogItemId},price={Price},quantity={Quantity})", username, catalogItemId, price, quantity);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var basketSpec = new BasketWithItemsSpecification(username);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

		_logger.LogDebug("AddItemToBasket(username={Username},catalogItemId={CatalogItemId},price={Price},quantity={Quantity}): basketSpec → {BasketSpec}", username, catalogItemId, price, quantity, basketSpec);
		_logger.LogInformation("AddItemToBasket(username,catalogItemId,price,quantity)={Username},{CatalogItemId},{Price},{Quantity}: database query executed in {Arg4} ms", username, catalogItemId, price, quantity, __logStart.ElapsedMilliseconds);
        if (basket == null)
		_logger.LogDebug("AddItemToBasket(username={Username},catalogItemId={CatalogItemId},price={Price},quantity={Quantity}): basket → {Basket}", username, catalogItemId, price, quantity, basket);
        {
            basket = new Basket(username);
            await _basketRepository.AddAsync(basket);
        }

        basket.AddItem(catalogItemId, price, quantity);

        await _basketRepository.UpdateAsync(basket);
		_logger.LogDebug("<<< Exiting AddItemToBasket(username={Username},catalogItemId={CatalogItemId},price={Price},quantity={Quantity})", username, catalogItemId, price, quantity);
        return basket;
    }

    public async Task DeleteBasketAsync(int basketId)
    {
		_logger.LogDebug(">>> Entering DeleteBasketAsync(basketId={BasketId})", basketId);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var basket = await _basketRepository.GetByIdAsync(basketId);
        Guard.Against.Null(basket, nameof(basket));
        await _basketRepository.DeleteAsync(basket);
		_logger.LogInformation("DeleteBasketAsync(basketId)={BasketId}: external service call took {Arg1} ms", basketId, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("<<< Exiting DeleteBasketAsync(basketId={BasketId})", basketId);
    }

    public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
    {
		_logger.LogDebug(">>> Entering SetQuantities(basketId={BasketId},quantities={Quantities})", basketId, quantities);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
        if (basket == null) return Result<Basket>.NotFound();
		_logger.LogDebug("SetQuantities(basketId={BasketId},quantities={Quantities}): basketSpec → {BasketSpec}", basketId, quantities, basketSpec);
		_logger.LogDebug("SetQuantities(basketId={BasketId},quantities={Quantities}): basket → {Basket}", basketId, quantities, basket);

        foreach (var item in basket.Items)
        {
            if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
			_logger.LogInformation("SetQuantities(basketId,quantities)={BasketId},{Quantities}: database query executed in {Arg2} ms", basketId, quantities, __logStart.ElapsedMilliseconds);
            {
                if (_logger != null) _logger.LogInformation($"Updating quantity of item ID:{item.Id} to {quantity}.");
                item.SetQuantity(quantity);
            }
        }
        basket.RemoveEmptyItems();
        await _basketRepository.UpdateAsync(basket);
		_logger.LogDebug("<<< Exiting SetQuantities(basketId={BasketId},quantities={Quantities})", basketId, quantities);
        return basket;
    }

    public async Task TransferBasketAsync(string anonymousId, string userName)
    {
		_logger.LogDebug(">>> Entering TransferBasketAsync(anonymousId={AnonymousId},userName={UserName})", anonymousId, userName);
		var __logStart = System.Diagnostics.Stopwatch.StartNew();
        var anonymousBasketSpec = new BasketWithItemsSpecification(anonymousId);
        var anonymousBasket = await _basketRepository.FirstOrDefaultAsync(anonymousBasketSpec);
        if (anonymousBasket == null) return;
        var userBasketSpec = new BasketWithItemsSpecification(userName);
		_logger.LogDebug("TransferBasketAsync(anonymousId={AnonymousId},userName={UserName}): anonymousBasketSpec → {AnonymousBasketSpec}", anonymousId, userName, anonymousBasketSpec);
		_logger.LogDebug("TransferBasketAsync(anonymousId={AnonymousId},userName={UserName}): anonymousBasket → {AnonymousBasket}", anonymousId, userName, anonymousBasket);
        var userBasket = await _basketRepository.FirstOrDefaultAsync(userBasketSpec);
        if (userBasket == null)
        {
			_logger.LogDebug("TransferBasketAsync(anonymousId={AnonymousId},userName={UserName}): userBasketSpec → {UserBasketSpec}", anonymousId, userName, userBasketSpec);
			_logger.LogInformation("TransferBasketAsync(anonymousId,userName)={AnonymousId},{UserName}: database query executed in {Arg2} ms", anonymousId, userName, __logStart.ElapsedMilliseconds);
            userBasket = new Basket(userName);
            await _basketRepository.AddAsync(userBasket);
			_logger.LogDebug("TransferBasketAsync(anonymousId={AnonymousId},userName={UserName}): userBasket → {UserBasket}", anonymousId, userName, userBasket);
        }
        foreach (var item in anonymousBasket.Items)
        {
            userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
        }
        await _basketRepository.UpdateAsync(userBasket);
        await _basketRepository.DeleteAsync(anonymousBasket);
		_logger.LogInformation("TransferBasketAsync(anonymousId,userName)={AnonymousId},{UserName}: external service call took {Arg2} ms", anonymousId, userName, __logStart.ElapsedMilliseconds);
		_logger.LogInformation("TransferBasketAsync(anonymousId,userName)={AnonymousId},{UserName}: update query executed in {Arg2} ms", anonymousId, userName, __logStart.ElapsedMilliseconds);
		_logger.LogDebug("<<< Exiting TransferBasketAsync(anonymousId={AnonymousId},userName={UserName})", anonymousId, userName);
    }
}
