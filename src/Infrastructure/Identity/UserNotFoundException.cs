using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.eShopWeb.Infrastructure.Identity;

public class UserNotFoundException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public UserNotFoundException(string userName) : base($"No user found with username: {userName}")
    {
	_logger.LogInformation("ENTERING: UserNotFoundException() method");
    }
}
