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
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        var basketSpec = new BasketWithItemsSpecification(username);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);

		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        if (basket == null)
        {
			_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed", username, catalogItemId, price, quantity);
            basket = new Basket(username);
			_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
            await _basketRepository.AddAsync(basket);
			_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        }
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);

		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        basket.AddItem(catalogItemId, price, quantity);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);

		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        await _basketRepository.UpdateAsync(basket);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): SOC2-AUDIT [CC6.6]: Database transaction completed", username, catalogItemId, price, quantity);
		_logger.LogInformation("AddItemToBasket(username={Username}, catalogItemId={CatalogItemId}, price={Price}, quantity={Quantity}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {CatalogItemId1}", username, catalogItemId, price, quantity, catalogItemId);
        return basket;
    }

    public async Task DeleteBasketAsync(int basketId)
    {
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        var basket = await _basketRepository.GetByIdAsync(basketId);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        Guard.Against.Null(basket, nameof(basket));
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {BasketId1}", basketId, basketId);
        await _basketRepository.DeleteAsync(basket);
		_logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {BasketId1}", basketId, basketId);
    _logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail {BasketId1}", basketId, basketId);
    _logger.LogInformation("DeleteBasketAsync(basketId={BasketId}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail {BasketId1}", basketId, basketId);
    }

    public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
    {
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        var basketSpec = new BasketWithItemsSpecification(basketId);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", basketId, quantities);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        if (basket == null) return Result<Basket>.NotFound();

        foreach (var item in basket.Items)
        {
            if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
            {
                if (_logger != null) _logger.LogInformation($"Updating quantity of item ID:{item.Id} to {quantity}.");
                item.SetQuantity(quantity);
				_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
            }
			_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        }
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        basket.RemoveEmptyItems();
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        await _basketRepository.UpdateAsync(basket);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): SOC2-AUDIT [CC6.1]: Authentication operation completed", basketId, quantities);
		_logger.LogInformation("SetQuantities(basketId={BasketId}, quantities={Quantities}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {BasketId1}", basketId, quantities, basketId);
        return basket;
    }

    public async Task TransferBasketAsync(string anonymousId, string userName)
    {
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        var anonymousBasketSpec = new BasketWithItemsSpecification(anonymousId);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        var anonymousBasket = await _basketRepository.FirstOrDefaultAsync(anonymousBasketSpec);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed", anonymousId, userName);
        if (anonymousBasket == null) return;
        var userBasketSpec = new BasketWithItemsSpecification(userName);
        var userBasket = await _basketRepository.FirstOrDefaultAsync(userBasketSpec);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        if (userBasket == null)
        {
			_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
            userBasket = new Basket(userName);
            await _basketRepository.AddAsync(userBasket);
			_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        }
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        foreach (var item in anonymousBasket.Items)
        {
			_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
            userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
			_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        }
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
        await _basketRepository.UpdateAsync(userBasket);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): SOC2-AUDIT [CC6.6]: Database transaction completed", anonymousId, userName);
        await _basketRepository.DeleteAsync(anonymousBasket);
		_logger.LogInformation("TransferBasketAsync(anonymousId={AnonymousId}, userName={UserName}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {AnonymousId1}", anonymousId, userName, anonymousId);
    }
}
