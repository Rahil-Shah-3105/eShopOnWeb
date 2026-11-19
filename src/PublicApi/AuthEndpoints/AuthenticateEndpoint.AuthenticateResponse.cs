using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.PublicApi.AuthEndpoints;

public class AuthenticateResponse : BaseResponse
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public AuthenticateResponse(Guid correlationId) : base(correlationId)
    {
	_logger.LogInformation("AuthenticateResponse(correlationId={CorrelationId}): SOC2-AUDIT [CC6.1]: Authentication operation completed", correlationId);
    _logger.LogInformation("AuthenticateResponse(correlationId={CorrelationId}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", correlationId);
	_logger.LogInformation("AuthenticateResponse(): SOC2-AUDIT [CC6.1]: Authentication operation completed");
    _logger.LogInformation("AuthenticateResponse(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed");
    }

    public AuthenticateResponse()
    {
    }
    public bool Result { get; set; } = false;
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsLockedOut { get; set; } = false;
    public bool IsNotAllowed { get; set; } = false;
    public bool RequiresTwoFactor { get; set; } = false;
}
