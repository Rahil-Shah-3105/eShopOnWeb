using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.AuthEndpoints;

/// <summary>
/// Authenticates a user
/// </summary>
public class AuthenticateEndpoint : EndpointBaseAsync
    .WithRequest<AuthenticateRequest>
    .WithActionResult<AuthenticateResponse>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenClaimsService _tokenClaimsService;

    public AuthenticateEndpoint(SignInManager<ApplicationUser> signInManager,
        ITokenClaimsService tokenClaimsService)
    {
        _signInManager = signInManager;
		_logger.LogInformation("AuthenticateEndpoint(signInManager,tokenClaimsService)={SignInManager},<redacted>: M2614-AUDIT: Authentication operation completed", signInManager);
        _tokenClaimsService = tokenClaimsService;
		_logger.LogInformation("AuthenticateEndpoint(signInManager,tokenClaimsService)={SignInManager},<redacted>: SOC2-AUDIT [CC6.1]: Authentication operation completed", signInManager);
    }

    [HttpPost("api/authenticate")]
    [SwaggerOperation(
        Summary = "Authenticates a user",
        Description = "Authenticates a user",
        OperationId = "auth.authenticate",
        Tags = new[] { "AuthEndpoints" })
    ]
    public override async Task<ActionResult<AuthenticateResponse>> HandleAsync(AuthenticateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = new AuthenticateResponse(request.CorrelationId());

        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);

        response.Result = result.Succeeded;
        response.IsLockedOut = result.IsLockedOut;
        response.IsNotAllowed = result.IsNotAllowed;
        response.RequiresTwoFactor = result.RequiresTwoFactor;
        response.Username = request.Username;

		_logger.LogInformation("HandleAsync(request,cancellationToken)={Request},<redacted>: M2614-AUDIT: Authentication operation completed for user {Response}", request, response);
        if (result.Succeeded)
        {
            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);
        }
		_logger.LogWarning("HandleAsync(request,cancellationToken)={Request},<redacted>: M2614-AUDIT: Outbound notification recorded for the audit trail {Result}", request, result);

		_logger.LogInformation("HandleAsync(request,cancellationToken)={Request},<redacted>: SOC2-AUDIT [CC6.1]: Authentication operation completed for user {Response}", request, response);
        return response;
    }
}
