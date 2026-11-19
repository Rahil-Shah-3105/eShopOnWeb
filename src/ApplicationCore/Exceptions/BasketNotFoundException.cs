using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class BasketNotFoundException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public BasketNotFoundException(int basketId) : base($"No basket found with id {basketId}")
    {
	_logger.LogError(">>> Entering BasketNotFoundException(basketId={BasketId})", basketId);
    _logger.LogWarning("<<< Exiting BasketNotFoundException(basketId={BasketId})", basketId);
    }
}
