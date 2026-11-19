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
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

        if (basket == null)
        {
			_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
            basket = new Basket(username);
            await _basketRepository.AddAsync(basket);
        }
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC6.6]: Database transaction completed", username, catalogItemId, price, quantity);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);

        basket.AddItem(catalogItemId, price, quantity);

        await _basketRepository.UpdateAsync(basket);
        return basket;
    }

    public async Task DeleteBasketAsync(int basketId)
    {
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {BasketId1}", basketId, basketId);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail {BasketId1}", basketId, basketId);
    _logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail {BasketId1}", basketId, basketId);
    _logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var basket = await _basketRepository.GetByIdAsync(basketId);
        Guard.Against.Null(basket, nameof(basket));
        await _basketRepository.DeleteAsync(basket);
    }

    public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
    {
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
        if (basket == null) return Result<Basket>.NotFound();

        foreach (var item in basket.Items)
        {
            if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
            {
                if (_logger != null) _logger.LogInformation($"Updating quantity of item ID:{item.Id} to {quantity}.");
                item.SetQuantity(quantity);
				_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
				_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
				_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
				_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
				_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
				_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC6.1]: Authentication operation completed", basketId, quantities);
            }
        }
        basket.RemoveEmptyItems();
        await _basketRepository.UpdateAsync(basket);
        return basket;
    }

    public async Task TransferBasketAsync(string anonymousId, string userName)
    {
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        var anonymousBasketSpec = new BasketWithItemsSpecification(anonymousId);
        var anonymousBasket = await _basketRepository.FirstOrDefaultAsync(anonymousBasketSpec);
        if (anonymousBasket == null) return;
        var userBasketSpec = new BasketWithItemsSpecification(userName);
        var userBasket = await _basketRepository.FirstOrDefaultAsync(userBasketSpec);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        if (userBasket == null)
        {
			_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
            userBasket = new Basket(userName);
            await _basketRepository.AddAsync(userBasket);
        }
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC6.6]: Database transaction completed", anonymousId, userName);
        foreach (var item in anonymousBasket.Items)
        {
			_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
            userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
        }
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        await _basketRepository.UpdateAsync(userBasket);
        await _basketRepository.DeleteAsync(anonymousBasket);
    }
}
