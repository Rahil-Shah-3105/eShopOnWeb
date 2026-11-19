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
		_logger.LogError(">>> Entering BaseResponse(correlationId={CorrelationId})", correlationId);
        base._correlationId = correlationId;
		_logger.LogDebug("BaseResponse(correlationId={CorrelationId}): base._correlationId → {Arg1}", correlationId, base._correlationId);
    _logger.LogWarning("<<< Exiting BaseResponse(correlationId={CorrelationId})", correlationId);
		_logger.LogError(">>> Entering BaseResponse()");
    _logger.LogWarning("<<< Exiting BaseResponse()");
    }

    public BaseResponse()
    {
    }
}
