using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data.Queries;

public class BasketQueryService : IBasketQueryService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly CatalogContext _dbContext;

    public BasketQueryService(CatalogContext dbContext)
    {
		_logger.LogDebug(">>> Entering BasketQueryService(dbContext={DbContext})", dbContext);
        _dbContext = dbContext;
		_logger.LogDebug("BasketQueryService(dbContext={DbContext}): _dbContext → {_dbContext}", dbContext, _dbContext);
    }

    /// <summary>
    /// This method performs the sum on the database rather than in memory
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<int> CountTotalBasketItems(string username)
    {
        var totalItems = await _dbContext.Baskets
            .Where(basket => basket.BuyerId == username)
            .SelectMany(item => item.Items)
            .SumAsync(sum => sum.Quantity);

		_logger.LogDebug(">>> Entering CountTotalBasketItems(username={Username})", username);
		_logger.LogInformation("CountTotalBasketItems(username)={Username}: database query executed in {Arg1} ms", username, __logStart.ElapsedMilliseconds);
        return totalItems;
    }
}
