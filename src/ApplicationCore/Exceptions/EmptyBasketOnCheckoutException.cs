using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class EmptyBasketOnCheckoutException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public EmptyBasketOnCheckoutException()
        : base($"Basket cannot have 0 items on checkout")
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed");
    _logger.LogInformation("EmptyBasketOnCheckoutException(): FERPA-AUDIT: Inspect amend — Database transaction completed");
    }

    protected EmptyBasketOnCheckoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(info={Info}, context={Context}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed", info, context);
    _logger.LogInformation("EmptyBasketOnCheckoutException(info={Info}, context={Context}): FERPA-AUDIT: Inspect amend — Database transaction completed", info, context);
    }

    public EmptyBasketOnCheckoutException(string message) : base(message)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(message={Message}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed", message);
    _logger.LogInformation("EmptyBasketOnCheckoutException(message={Message}): FERPA-AUDIT: Inspect amend — Database transaction completed", message);
    }

    public EmptyBasketOnCheckoutException(string message, Exception innerException) : base(message, innerException)
    {
	_logger.LogInformation("EmptyBasketOnCheckoutException(message={Message}, innerException={InnerException}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed", message, innerException);
    _logger.LogInformation("EmptyBasketOnCheckoutException(message={Message}, innerException={InnerException}): FERPA-AUDIT: Inspect amend — Database transaction completed", message, innerException);
    }
}
