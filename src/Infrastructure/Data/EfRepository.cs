using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public EfRepository(CatalogContext dbContext) : base(dbContext)
    {
	_logger.LogDebug(">>> Entering EfRepository(dbContext={DbContext})", dbContext);
	_logger.LogDebug("<<< Exiting EfRepository(dbContext={DbContext})", dbContext);
    }
}
