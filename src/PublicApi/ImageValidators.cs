using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi;

public static class ImageValidators
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private const int ImageMaximumBytes = 512000;

    public static bool IsValidImage(this byte[] postedFile, string fileName)
    {
		_logger.LogInformation("ENTERING: IsValidImage() method");
        return postedFile != null && postedFile.Length > 0 && postedFile.Length <= ImageMaximumBytes && IsExtensionValid(fileName);
    }

    private static bool IsExtensionValid(string fileName)
    {
		_logger.LogInformation("ENTERING: IsExtensionValid() method");
        var extension = Path.GetExtension(fileName);

        return string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);
    }
}
