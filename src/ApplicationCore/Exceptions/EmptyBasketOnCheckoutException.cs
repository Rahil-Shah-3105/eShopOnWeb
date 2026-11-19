using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class EmptyBasketOnCheckoutException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public EmptyBasketOnCheckoutException()
        : base($"Basket cannot have 0 items on checkout")
    {
	_logger.LogError(">>> Entering EmptyBasketOnCheckoutException()");
    _logger.LogWarning("<<< Exiting EmptyBasketOnCheckoutException()");
    }

    protected EmptyBasketOnCheckoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
	_logger.LogError(">>> Entering EmptyBasketOnCheckoutException(info={Info},context={Context})", info, context);
    _logger.LogWarning("<<< Exiting EmptyBasketOnCheckoutException(info={Info},context={Context})", info, context);
    }

    public EmptyBasketOnCheckoutException(string message) : base(message)
    {
	_logger.LogError(">>> Entering EmptyBasketOnCheckoutException(message={Message})", message);
    _logger.LogWarning("<<< Exiting EmptyBasketOnCheckoutException(message={Message})", message);
    }

    public EmptyBasketOnCheckoutException(string message, Exception innerException) : base(message, innerException)
    {
	_logger.LogError(">>> Entering EmptyBasketOnCheckoutException(message={Message},innerException={InnerException})", message, innerException);
    _logger.LogWarning("<<< Exiting EmptyBasketOnCheckoutException(message={Message},innerException={InnerException})", message, innerException);
    }
}
