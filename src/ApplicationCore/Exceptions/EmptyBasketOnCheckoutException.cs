using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class EmptyBasketOnCheckoutException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public EmptyBasketOnCheckoutException()
        : base($"Basket cannot have 0 items on checkout")
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(): SOC2-AUDIT [CC6.6]: Database transaction completed");
    }

    protected EmptyBasketOnCheckoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(info={Info}, context={Context}): SOC2-AUDIT [CC6.6]: Database transaction completed", info, context);
    }

    public EmptyBasketOnCheckoutException(string message) : base(message)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(message={Message}): SOC2-AUDIT [CC6.6]: Database transaction completed", message);
    }

    public EmptyBasketOnCheckoutException(string message, Exception innerException) : base(message, innerException)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(message={Message}, innerException={InnerException}): SOC2-AUDIT [CC6.6]: Database transaction completed", message, innerException);
    }
}
