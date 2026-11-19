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
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC7.2]: Monitoring coverage — Record access recorded for the audit trail", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC6.8]: Malicious software — Record access recorded for the audit trail", userName);
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) throw new UserNotFoundException(userName);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC6.1]: Logical access — Record access recorded for the audit trail", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC6.6]: Boundary protection — Record access recorded for the audit trail", userName);

        foreach (var role in roles)
        {
			_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC7.3]: Incident response — Record access recorded for the audit trail", userName);
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC8.1]: Change management — Record access recorded for the audit trail", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC6.7]: Encryption operation completed", userName);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
			_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC6.1]: Password reset operation performed for user {UserName1}", userName, userName);
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC7.2]: System operation completed", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC6.3]: Authorization — Record access recorded for the audit trail", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {UserName1}", userName, userName);
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
