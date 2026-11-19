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
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(await GetUser(useCache: true));
    }

    private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = false)
    {
        var now = DateTimeOffset.Now;
        if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
        {
            return _cachedUser;
        }

        _cachedUser = await FetchUser();
		_logger.LogInformation("GetUser(useCache={UseCache}): CCPA-AUDIT: Risk assessment — Record access recorded for the audit trail", useCache);
        _userLastCheck = now;
		_logger.LogInformation("GetUser(useCache={UseCache}): CCPA-AUDIT: Cyber audit — Record access recorded for the audit trail", useCache);

		_logger.LogInformation("GetUser(useCache={UseCache}): CCPA-AUDIT: Consumer rights — Record access recorded for the audit trail", useCache);
        return _cachedUser;
    }

    private async Task<ClaimsPrincipal> FetchUser()
    {
        UserInfo user = null;

        try
        {
            _logger.LogInformation("Fetching user details from web api.");
            user = await _httpClient.GetFromJsonAsync<UserInfo>("User");
        }
        catch (Exception exc)
        {
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

		_logger.LogInformation("FetchUser(): CCPA-AUDIT: Risk assessment — Record access recorded for the audit trail {Identity}", identity);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
		_logger.LogInformation("FetchUser(): CCPA-AUDIT: Cyber audit — Record access recorded for the audit trail {Identity}", identity);

		_logger.LogInformation("FetchUser(): CCPA-AUDIT: Consumer rights — Record access recorded for the audit trail {Identity}", identity);
		_logger.LogInformation("FetchUser(): CCPA-AUDIT: Admt decision — Record access recorded for the audit trail {Identity}", identity);
        return new ClaimsPrincipal(identity);
    }
}
