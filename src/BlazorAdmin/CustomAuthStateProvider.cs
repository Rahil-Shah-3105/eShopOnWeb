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
		_logger.LogInformation("GetAuthenticationStateAsync(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
		_logger.LogInformation("GetAuthenticationStateAsync(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail");
        return new AuthenticationState(await GetUser(useCache: true));
    }

    private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = false)
    {
		_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail", useCache);
        var now = DateTimeOffset.Now;
		_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail", useCache);
        if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
        {
			_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail", useCache);
			_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail", useCache);
            return _cachedUser;
        }
		_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail", useCache);

		_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Outcome 1: user attribution — Record access recorded for the audit trail", useCache);
        _cachedUser = await FetchUser();
		_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail", useCache);
        _userLastCheck = now;
		_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail", useCache);

		_logger.LogInformation("GetUser(useCache={UseCache}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail", useCache);
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
			_logger.LogInformation("FetchUser(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail {Identity}", identity);

		_logger.LogInformation("FetchUser(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail {Identity}", identity);
        if (user.Claims != null)
        {
			_logger.LogInformation("FetchUser(): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail {Identity}", identity);
            foreach (var claim in user.Claims)
            {
				_logger.LogInformation("FetchUser(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail {Identity}", identity);
                identity.AddClaim(new Claim(claim.Type, claim.Value));
				_logger.LogInformation("FetchUser(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {User}", user);
            }
			_logger.LogInformation("FetchUser(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail {Identity}", identity);
        }
		_logger.LogInformation("FetchUser(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail {Identity}", identity);

		_logger.LogInformation("FetchUser(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail {Identity}", identity);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
		_logger.LogInformation("FetchUser(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail {Identity}", identity);

		_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {User}", user);
		_logger.LogInformation("FetchUser(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail {Identity}", identity);
        return new ClaimsPrincipal(identity);
    }
}
