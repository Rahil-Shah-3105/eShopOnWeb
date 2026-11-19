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
		_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail", userName);
        var tokenHandler = new JwtSecurityTokenHandler();
		_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail", userName);
        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail", userName);
        var user = await _userManager.FindByNameAsync(userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail", userName);
        if (user == null) throw new UserNotFoundException(userName);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
		_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail", userName);

		_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {UserName1}", userName, userName);
        foreach (var role in roles)
        {
			_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail", userName);
            claims.Add(new Claim(ClaimTypes.Role, role));
			_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail", userName);
        }
		_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail", userName);

		_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail", userName);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
			_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail", userName);
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
			_logger.LogInformation("GetTokenAsync(userName={UserName}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail", userName);
        var token = tokenHandler.CreateToken(tokenDescriptor);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {UserName1}", userName, userName);
        return tokenHandler.WriteToken(token);
    }
}
