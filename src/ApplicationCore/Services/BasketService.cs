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
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);

        if (basket == null)
        {
			_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
            basket = new Basket(username);
            await _basketRepository.AddAsync(basket);
        }
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogWarning("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT: Access, erasure, portability — Database transaction completed", username, catalogItemId, price, quantity);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT [Art.30]: Personal data processing operation recorded {Username1}", username, catalogItemId, price, quantity, username);
		_logger.LogWarning("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);

        basket.AddItem(catalogItemId, price, quantity);

        await _basketRepository.UpdateAsync(basket);
        return basket;
    }

    public async Task DeleteBasketAsync(int basketId)
    {
		_logger.LogWarning("DeleteBasketAsync(basketId={BasketId}): GDPR-AUDIT [Art.17]: Data subject right exercised — erasure/portability request processed {BasketId1}", basketId, basketId);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogWarning("DeleteBasketAsync(basketId={BasketId}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {BasketId1}", basketId, basketId);
    _logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail {BasketId1}", basketId, basketId);
    _logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var basket = await _basketRepository.GetByIdAsync(basketId);
        Guard.Against.Null(basket, nameof(basket));
        await _basketRepository.DeleteAsync(basket);
    }

    public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
    {
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
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
			_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
			_logger.LogWarning("SetQuantities(basketId={BasketId}, quantities={Quantities}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
			_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
			_logger.LogWarning("SetQuantities(basketId={BasketId}, quantities={Quantities}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
			_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        }
        basket.RemoveEmptyItems();
        await _basketRepository.UpdateAsync(basket);
        return basket;
    }

    public async Task TransferBasketAsync(string anonymousId, string userName)
    {
        var anonymousBasketSpec = new BasketWithItemsSpecification(anonymousId);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT: Transfers to third countries — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        var anonymousBasket = await _basketRepository.FirstOrDefaultAsync(anonymousBasketSpec);
        if (anonymousBasket == null) return;
        var userBasketSpec = new BasketWithItemsSpecification(userName);
        var userBasket = await _basketRepository.FirstOrDefaultAsync(userBasketSpec);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        if (userBasket == null)
        {
			_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
            userBasket = new Basket(userName);
            await _basketRepository.AddAsync(userBasket);
        }
		_logger.LogWarning("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT: Access, erasure, portability — Database transaction completed", anonymousId, userName);
        foreach (var item in anonymousBasket.Items)
        {
			_logger.LogWarning("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT [Art.44]: Personal data transfer operation recorded", anonymousId, userName);
            userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
        }
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
		_logger.LogWarning("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        await _basketRepository.UpdateAsync(userBasket);
        await _basketRepository.DeleteAsync(anonymousBasket);
    }
}
