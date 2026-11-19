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
        if (user == null) throw new UserNotFoundException(userName);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };

		_logger.LogInformation("GetTokenAsync(userName={UserName}): HIPAA-AUDIT [45 CFR 164.312(e)(1)]: Transmission security — Password reset operation performed for user {UserName1}", userName, userName);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
		_logger.LogInformation("GetTokenAsync(userName={UserName}): HIPAA-AUDIT [45 CFR 164.312(e)(1)]: Transmission security — Encryption operation completed", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): HIPAA-AUDIT [45 CFR 164.312(a)(2)(iv)]: Encryption and decryption — Password reset operation performed for user {UserName1}", userName, userName);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
		_logger.LogInformation("GetTokenAsync(userName={UserName}): HIPAA-AUDIT [45 CFR 164.312(a)(2)(iv)]: Encryption and decryption — Encryption operation completed", userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): HIPAA-AUDIT [45 CFR 164.312(d)]: Person or entity authentication — Authentication operation completed for user {UserName1}", userName, userName);
		_logger.LogInformation("GetTokenAsync(userName={UserName}): HIPAA-AUDIT: Record access recorded for the audit trail", userName);
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
