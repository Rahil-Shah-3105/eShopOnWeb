using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class EmptyBasketOnCheckoutException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public EmptyBasketOnCheckoutException()
        : base($"Basket cannot have 0 items on checkout")
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException()=null): CCPA-AUDIT: Database transaction completed");
	_logger.LogInformation("EmptyBasketOnCheckoutException(info,context)={Info},{Context}: CCPA-AUDIT: Database transaction completed", info, context);
    }

    protected EmptyBasketOnCheckoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(message)={Message}): CCPA-AUDIT: Database transaction completed", message);
    }

    public EmptyBasketOnCheckoutException(string message) : base(message)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(message,innerException)={Message},{InnerException}: CCPA-AUDIT: Database transaction completed", message, innerException);
    }

    public EmptyBasketOnCheckoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
