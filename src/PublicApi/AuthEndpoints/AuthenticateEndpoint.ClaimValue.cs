using Microsoft.Extensions.Logging;
﻿namespace Microsoft.eShopWeb.PublicApi.AuthEndpoints;

public class ClaimValue
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public ClaimValue()
    {
	_logger.LogDebug(">>> Entering ClaimValue()");
	_logger.LogDebug("<<< Exiting ClaimValue()");
    }

    public ClaimValue(string type, string value)
    {
		_logger.LogDebug(">>> Entering ClaimValue(type={Type},value={Value})", type, value);
        Type = type;
        Value = value;
		_logger.LogDebug("ClaimValue(type={Type},value={Value}): Value → {Value1}", type, value, Value);
		_logger.LogDebug("<<< Exiting ClaimValue(type={Type},value={Value})", type, value);
    }

    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
