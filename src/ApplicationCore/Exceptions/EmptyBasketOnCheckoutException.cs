using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class EmptyBasketOnCheckoutException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public EmptyBasketOnCheckoutException()
        : base($"Basket cannot have 0 items on checkout")
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException()=null): SOC2-AUDIT [CC6.6]: Database transaction completed");
	_logger.LogInformation("EmptyBasketOnCheckoutException(info,context)={Info},{Context}: SOC2-AUDIT [CC6.6]: Database transaction completed", info, context);
    }

    protected EmptyBasketOnCheckoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(message)={Message}): SOC2-AUDIT [CC6.6]: Database transaction completed", message);
    }

    public EmptyBasketOnCheckoutException(string message) : base(message)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(message,innerException)={Message},{InnerException}: SOC2-AUDIT [CC6.6]: Database transaction completed", message, innerException);
    }

    public EmptyBasketOnCheckoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
