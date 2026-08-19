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
		_logger.LogDebug(">>> Entering AuthenticateEndpoint(signInManager={SignInManager},tokenClaimsService=<redacted>)", signInManager);
        _signInManager = signInManager;
        _tokenClaimsService = tokenClaimsService;
		_logger.LogDebug("AuthenticateEndpoint(signInManager={SignInManager},tokenClaimsService=<redacted>): _signInManager → {_signInManager}", signInManager, _signInManager);
		_logger.LogDebug("AuthenticateEndpoint(signInManager={SignInManager},tokenClaimsService=<redacted>): _tokenClaimsService → {_tokenClaimsService}", signInManager, _tokenClaimsService);
		_logger.LogDebug("<<< Exiting AuthenticateEndpoint(signInManager={SignInManager},tokenClaimsService=<redacted>)", signInManager);
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
		_logger.LogDebug(">>> Entering HandleAsync(request={Request},cancellationToken=<redacted>)", request);
        var response = new AuthenticateResponse(request.CorrelationId());

		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response → {Response}", request, response);
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): result → {Result}", request, result);

        response.Result = result.Succeeded;
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.Result → {Arg2}", request, response.Result);
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.IsLockedOut → {Arg2}", request, response.IsLockedOut);
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.IsNotAllowed → {Arg2}", request, response.IsNotAllowed);
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.RequiresTwoFactor → {Arg2}", request, response.RequiresTwoFactor);
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.Username → {Arg2}", request, response.Username);
        response.IsLockedOut = result.IsLockedOut;
        response.IsNotAllowed = result.IsNotAllowed;
        response.RequiresTwoFactor = result.RequiresTwoFactor;
        response.Username = request.Username;

        if (result.Succeeded)
        {
            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);
        }

		_logger.LogDebug("<<< Exiting HandleAsync(request={Request},cancellationToken=<redacted>)", request);
        return response;
    }
}
