using Microsoft.Extensions.Logging;
﻿using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class DuplicateException : Exception
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public DuplicateException(string message) : base(message)
    {
	_logger.LogDebug(">>> Entering DuplicateException(message={Message})", message);

	_logger.LogDebug("<<< Exiting DuplicateException(message={Message})", message);
    }

}
