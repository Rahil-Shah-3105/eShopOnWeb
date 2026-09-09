using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi;

/// <summary>
/// Base class used by API responses
/// </summary>
public abstract class BaseResponse : BaseMessage
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public BaseResponse(Guid correlationId) : base()
    {
		_logger.LogInformation("ENTERING: BaseResponse() method");
        base._correlationId = correlationId;
    }

    public BaseResponse()
    {
    }
}
