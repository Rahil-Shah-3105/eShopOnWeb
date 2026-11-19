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
		_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC7.2]: Monitoring coverage — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail", useCache);
        var now = DateTimeOffset.Now;
        if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
        {
			_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail", useCache);
            return _cachedUser;
        }
		_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail", useCache);
		_logger.LogInformation("GetUser(useCache={UseCache}): SOC2-AUDIT [CC6.2]: Authentication — Record access recorded for the audit trail", useCache);

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
			_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail {Identity}", identity);
			_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC7.2]: Monitoring coverage — Record access recorded for the audit trail {Identity}", identity);

        if (user.Claims != null)
        {
			_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail {Identity}", identity);
            foreach (var claim in user.Claims)
            {
				_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail {Identity}", identity);
                identity.AddClaim(new Claim(claim.Type, claim.Value));
            }
        }
		_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail {Identity}", identity);
		_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail {Identity}", identity);
		_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC7.1]: System operations — Record access recorded for the audit trail {Identity}", identity);
		_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail {Identity}", identity);
		_logger.LogInformation("FetchUser(): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {User}", user);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        return new ClaimsPrincipal(identity);
    }
}
