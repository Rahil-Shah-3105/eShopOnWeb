using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorShared.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    // TODO: Get Default Cache Duration from Config
    private static readonly TimeSpan UserCacheRefreshInterval = TimeSpan.FromSeconds(60);

    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomAuthStateProvider> _logger;

    private DateTimeOffset _userLastCheck = DateTimeOffset.FromUnixTimeSeconds(0);
    private ClaimsPrincipal _cachedUser = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(HttpClient httpClient,
        ILogger<CustomAuthStateProvider> logger)
    {
        _httpClient = httpClient;
		_logger.LogDebug("CustomAuthStateProvider(httpClient={HttpClient},logger={Logger}): _httpClient → {_httpClient}", httpClient, logger, _httpClient);
        _logger = logger;
		_logger.LogError(">>> Entering CustomAuthStateProvider(httpClient={HttpClient},logger={Logger})", httpClient, logger);
    _logger.LogWarning("<<< Exiting CustomAuthStateProvider(httpClient={HttpClient},logger={Logger})", httpClient, logger);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
		_logger.LogError(">>> Entering GetAuthenticationStateAsync()");
        _logger.LogWarning("<<< Exiting GetAuthenticationStateAsync()");
        return new AuthenticationState(await GetUser(useCache: true));
    }

    private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = false)
    {
		_logger.LogError(">>> Entering GetUser(useCache={UseCache})", useCache);
        var now = DateTimeOffset.Now;
        if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
        {
            return _cachedUser;
        }

        _cachedUser = await FetchUser();
        _userLastCheck = now;

		_logger.LogDebug("GetUser(useCache={UseCache}): _userLastCheck → {_userLastCheck}", useCache, _userLastCheck);
		_logger.LogWarning("<<< Exiting GetUser(useCache={UseCache})", useCache);
        return _cachedUser;
    }

    private async Task<ClaimsPrincipal> FetchUser()
    {
		_logger.LogError(">>> Entering FetchUser()");
        var __logStart = System.Diagnostics.Stopwatch.StartNew();
        UserInfo user = null;

        try
        {
            _logger.LogInformation("Fetching user details from web api.");
            user = await _httpClient.GetFromJsonAsync<UserInfo>("User");
			_logger.LogError("FetchUser(): user → {User}", user);
        }
        catch (Exception exc)
        {
			_logger.LogError("FetchUser(): external service call took {Arg0} ms", __logStart.ElapsedMilliseconds);
            _logger.LogWarning(exc, "Fetching user failed.");
        }

        if (user == null || !user.IsAuthenticated)
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        var identity = new ClaimsIdentity(
            nameof(CustomAuthStateProvider),
            user.NameClaimType,
            user.RoleClaimType);

        if (user.Claims != null)
        {
            foreach (var claim in user.Claims)
            {
                identity.AddClaim(new Claim(claim.Type, claim.Value));
            }
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

		_logger.LogWarning("<<< Exiting FetchUser()");
        return new ClaimsPrincipal(identity);
    }
}
