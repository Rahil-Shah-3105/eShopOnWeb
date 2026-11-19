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
		_logger.LogError(">>> Entering AuthenticateEndpoint(signInManager={SignInManager},tokenClaimsService={TokenClaimsService})", signInManager, tokenClaimsService);
        _signInManager = signInManager;
		_logger.LogDebug("AuthenticateEndpoint(signInManager={SignInManager},tokenClaimsService={TokenClaimsService}): _signInManager → {_signInManager}", signInManager, tokenClaimsService, _signInManager);
        _tokenClaimsService = tokenClaimsService;
		_logger.LogDebug("AuthenticateEndpoint(signInManager={SignInManager},tokenClaimsService={TokenClaimsService}): _tokenClaimsService → {_tokenClaimsService}", signInManager, tokenClaimsService, _tokenClaimsService);
		_logger.LogWarning("<<< Exiting AuthenticateEndpoint(signInManager={SignInManager},tokenClaimsService={TokenClaimsService})", signInManager, tokenClaimsService);
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
		_logger.LogError(">>> Entering HandleAsync(request={Request},cancellationToken=<redacted>)", request);
        var response = new AuthenticateResponse(request.CorrelationId());

		_logger.LogError("HandleAsync(request={Request},cancellationToken=<redacted>): response → {Response}", request, response);
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);
		_logger.LogError("HandleAsync(request={Request},cancellationToken=<redacted>): result → {Result}", request, result);

        response.Result = result.Succeeded;
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.Result → {Arg2}", request, response.Result);
        response.IsLockedOut = result.IsLockedOut;
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.IsLockedOut → {Arg2}", request, response.IsLockedOut);
        response.IsNotAllowed = result.IsNotAllowed;
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.IsNotAllowed → {Arg2}", request, response.IsNotAllowed);
        response.RequiresTwoFactor = result.RequiresTwoFactor;
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.RequiresTwoFactor → {Arg2}", request, response.RequiresTwoFactor);
        response.Username = request.Username;
		_logger.LogDebug("HandleAsync(request={Request},cancellationToken=<redacted>): response.Username → {Arg2}", request, response.Username);

        if (result.Succeeded)
        {
            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);
        }

		_logger.LogWarning("<<< Exiting HandleAsync(request={Request},cancellationToken=<redacted>)", request);
        return response;
    }
}
