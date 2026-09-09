using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class EmptyBasketOnCheckoutException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public EmptyBasketOnCheckoutException()
        : base($"Basket cannot have 0 items on checkout")
    {
	_logger.LogInformation("ENTERING: EmptyBasketOnCheckoutException() method");
    }

    protected EmptyBasketOnCheckoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }

    public EmptyBasketOnCheckoutException(string message) : base(message)
    {
    }

    public EmptyBasketOnCheckoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
