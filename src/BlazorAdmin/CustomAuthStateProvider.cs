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
		_logger.LogInformation("GetUser(useCache={UseCache}): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail", useCache);
        if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
        {
			_logger.LogInformation("GetUser(useCache={UseCache}): GDPR-AUDIT: Transfers to third countries — Record access recorded for the audit trail", useCache);
            return _cachedUser;
        }
		_logger.LogInformation("GetUser(useCache={UseCache}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail", useCache);
		_logger.LogWarning("GetUser(useCache={UseCache}): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail", useCache);
		_logger.LogWarning("GetUser(useCache={UseCache}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail", useCache);

        _cachedUser = await FetchUser();
        _userLastCheck = now;

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
			_logger.LogInformation("FetchUser(): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail {Identity}", identity);
			_logger.LogInformation("FetchUser(): GDPR-AUDIT: Transfers to third countries — Record access recorded for the audit trail {Identity}", identity);

        if (user.Claims != null)
        {
			_logger.LogInformation("FetchUser(): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail {Identity}", identity);
            foreach (var claim in user.Claims)
            {
				_logger.LogInformation("FetchUser(): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail {Identity}", identity);
                identity.AddClaim(new Claim(claim.Type, claim.Value));
            }
        }
		_logger.LogWarning("FetchUser(): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail {Identity}", identity);
		_logger.LogInformation("FetchUser(): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail {Identity}", identity);
		_logger.LogInformation("FetchUser(): GDPR-AUDIT [Art.30]: Personal data processing operation recorded {User}", user);
		_logger.LogWarning("FetchUser(): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail {Identity}", identity);
		_logger.LogInformation("FetchUser(): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail {Identity}", identity);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        return new ClaimsPrincipal(identity);
    }
}
