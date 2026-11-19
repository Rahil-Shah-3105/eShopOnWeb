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
		_logger.LogError(">>> Entering IsValidImage(postedFile={PostedFile},fileName={FileName})", postedFile, fileName);
        _logger.LogWarning("<<< Exiting IsValidImage(postedFile={PostedFile},fileName={FileName})", postedFile, fileName);
        return postedFile != null && postedFile.Length > 0 && postedFile.Length <= ImageMaximumBytes && IsExtensionValid(fileName);
    }

    private static bool IsExtensionValid(string fileName)
    {
		_logger.LogError(">>> Entering IsExtensionValid(fileName={FileName})", fileName);
        var extension = Path.GetExtension(fileName);

		_logger.LogError("IsExtensionValid(fileName={FileName}): extension → {Extension}", fileName, extension);
		_logger.LogWarning("<<< Exiting IsExtensionValid(fileName={FileName})", fileName);
        return string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);
    }
}
