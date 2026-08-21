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
		_logger.LogDebug(">>> Entering IdentityTokenClaimService(userManager={UserManager})", userManager);
        _userManager = userManager;
		_logger.LogDebug("IdentityTokenClaimService(userManager={UserManager}): _userManager → {_userManager}", userManager, _userManager);
    }

    public async Task<string> GetTokenAsync(string userName)
    {
		_logger.LogDebug(">>> Entering GetTokenAsync(userName={UserName})", userName);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
		_logger.LogWarning("GetTokenAsync(userName={UserName}): key → {Key}", userName, key);
        var user = await _userManager.FindByNameAsync(userName);
		_logger.LogDebug("GetTokenAsync(userName={UserName}): user → {User}", userName, user);
        if (user == null) throw new UserNotFoundException(userName);
        var roles = await _userManager.GetRolesAsync(user);
		_logger.LogDebug("GetTokenAsync(userName={UserName}): roles → {Roles}", userName, roles);
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
		_logger.LogDebug("GetTokenAsync(userName={UserName}): claims → {Claims}", userName, claims);
		_logger.LogDebug("GetTokenAsync(userName={UserName}): tokenHandler → {TokenHandler}", userName, tokenHandler);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
		_logger.LogWarning("GetTokenAsync(userName={UserName}): token → {Token}", userName, token);
		_logger.LogDebug("<<< Exiting GetTokenAsync(userName={UserName})", userName);
        return tokenHandler.WriteToken(token);
    }
}
