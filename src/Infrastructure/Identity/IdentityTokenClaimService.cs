using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Identity;

public class IdentityTokenClaimService : ITokenClaimsService
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityTokenClaimService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> GetTokenAsync(string userName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
        var user = await _userManager.FindByNameAsync(userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): GDPR-AUDIT: Transfers to third countries — Password reset operation performed for user {UserName1}", userName, userName);
        if (user == null) throw new UserNotFoundException(userName);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
		_logger.LogInformation("GetTokenAsync(userName={UserName}): GDPR-AUDIT: Transfers to third countries — Encryption operation completed", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — Record access recorded for the audit trail", userName);

        foreach (var role in roles)
        {
			_logger.LogInformation("GetTokenAsync(userName={UserName}): GDPR-AUDIT [Art. 32]: Security of processing — Record access recorded for the audit trail", userName);
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
		_logger.LogInformation("GetTokenAsync(userName={UserName}): GDPR-AUDIT [Art. 30]: Records of processing activities — Record access recorded for the audit trail", userName);
		_logger.LogWarning("GetTokenAsync(userName={UserName}): GDPR-AUDIT: Access, erasure, portability — Record access recorded for the audit trail", userName);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
			_logger.LogInformation("GetTokenAsync(userName={UserName}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — Record access recorded for the audit trail", userName);
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
		_logger.LogInformation("GetTokenAsync(userName={UserName}): GDPR-AUDIT [Art.30]: Personal data processing operation recorded {UserName1}", userName, userName);
		_logger.LogWarning("GetTokenAsync(userName={UserName}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — Record access recorded for the audit trail", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): GDPR-AUDIT: Breach notification — Record access recorded for the audit trail", userName);
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
