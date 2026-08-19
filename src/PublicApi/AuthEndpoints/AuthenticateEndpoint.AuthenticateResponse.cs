using Microsoft.Extensions.Logging;
﻿using System;

namespace Microsoft.eShopWeb.PublicApi.AuthEndpoints;

public class AuthenticateResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public AuthenticateResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogDebug(">>> Entering AuthenticateResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogDebug("<<< Exiting AuthenticateResponse(correlationId={CorrelationId})", correlationId);
	_logger.LogDebug(">>> Entering AuthenticateResponse()");
    }

    public AuthenticateResponse()
    {
	_logger.LogDebug("<<< Exiting AuthenticateResponse()");
    }
    public bool Result { get; set; } = false;
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsLockedOut { get; set; } = false;
    public bool IsNotAllowed { get; set; } = false;
    public bool RequiresTwoFactor { get; set; } = false;
}
