using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi;

/// <summary>
/// Base class used by API requests
/// </summary>
public abstract class BaseMessage
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    /// <summary>
    /// Unique Identifier used by logging
    /// </summary>
    protected Guid _correlationId = Guid.NewGuid();
    public Guid CorrelationId()
    {
        _logger.LogError(">>> Entering CorrelationId()");
        _logger.LogWarning("<<< Exiting CorrelationId()");
        return _correlationId;
    }
}
