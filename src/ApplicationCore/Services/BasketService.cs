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
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);

		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): NIS2-AUDIT: Security events — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        await _basketRepository.UpdateAsync(basket);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        return basket;
    }

    public async Task DeleteBasketAsync(int basketId)
    {
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var basket = await _basketRepository.GetByIdAsync(basketId);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): NIS2-AUDIT: Security events — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        Guard.Against.Null(basket, nameof(basket));
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        await _basketRepository.DeleteAsync(basket);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail {BasketId1}", basketId, basketId);
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
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        basket.RemoveEmptyItems();
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): NIS2-AUDIT: Security events — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        await _basketRepository.UpdateAsync(basket);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
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
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): NIS2-AUDIT [Art. 21(2)(d)]: Supply chain — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        foreach (var item in anonymousBasket.Items)
        {
            userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
        }
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): NIS2-AUDIT: Security events — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        await _basketRepository.UpdateAsync(userBasket);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): NIS2-AUDIT [Art. 20]: Governance — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        await _basketRepository.DeleteAsync(anonymousBasket);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): NIS2-AUDIT [Art. 21(2)(c)]: Business continuity — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
    }
}
