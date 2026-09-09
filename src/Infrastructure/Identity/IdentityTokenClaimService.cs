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
		_logger.LogInformation("ENTERING: IdentityTokenClaimService() method");
        _userManager = userManager;
    }

    public async Task<string> GetTokenAsync(string userName)
    {
		_logger.LogInformation("ENTERING: GetTokenAsync() method");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) throw new UserNotFoundException(userName);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

		_logger.LogInformation("GetTokenAsync(userName)={UserName}): GDPR-AUDIT: Encryption operation completed", userName);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
			_logger.LogWarning("GetTokenAsync(userName)={UserName}): GDPR-AUDIT: Password reset operation performed for user {UserName1}", userName, userName);
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
		_logger.LogWarning("GetTokenAsync(userName)={UserName}): GDPR-AUDIT: Outbound notification recorded for the audit trail", userName);
		_logger.LogInformation("GetTokenAsync(userName)={UserName}): GDPR-AUDIT [Art.30]: Personal data processing operation recorded {UserName1}", userName, userName);
        return tokenHandler.WriteToken(token);
    }
}
