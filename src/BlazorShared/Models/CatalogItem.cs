using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using BlazorInputFile;
using Microsoft.Extensions.Logging;

namespace BlazorShared.Models;

public class CatalogItem
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public int Id { get; set; }

    public int CatalogTypeId { get; set; }
    public string CatalogType { get; set; } = "NotSet";

    public int CatalogBrandId { get; set; }
    public string CatalogBrand { get; set; } = "NotSet";

    [Required(ErrorMessage = "The Name field is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Description field is required")]
    public string Description { get; set; }

    // decimal(18,2)
    [RegularExpression(@"^\d+(\.\d{0,2})*$", ErrorMessage = "The field Price must be a positive number with maximum two decimals.")]
    [Range(0.01, 1000)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    public string PictureUri { get; set; }
    public string PictureBase64 { get; set; }
    public string PictureName { get; set; }

    private const int ImageMaximumBytes = 512000;

    public static string IsValidImage(string pictureName, string pictureBase64)
    {
		_logger.LogDebug(">>> Entering IsValidImage(pictureName={PictureName},pictureBase64={PictureBase64})", pictureName, pictureBase64);
        if (string.IsNullOrEmpty(pictureBase64))
        {
            return "File not found!";
        }
        var fileData = Convert.FromBase64String(pictureBase64);
		_logger.LogDebug("IsValidImage(pictureName={PictureName},pictureBase64={PictureBase64}): fileData → {FileData}", pictureName, pictureBase64, fileData);

        if (fileData.Length <= 0)
        {
            return "File length is 0!";
        }

        if (fileData.Length > ImageMaximumBytes)
        {
            return "Maximum length is 512KB";
        }

        if (!IsExtensionValid(pictureName))
        {
		_logger.LogDebug("<<< Exiting IsValidImage(pictureName={PictureName},pictureBase64={PictureBase64})", pictureName, pictureBase64);
            return "File is not image";
        }

        return null;
    }

    public static async Task<string> DataToBase64(IFileListEntry fileItem)
    {
		_logger.LogDebug(">>> Entering DataToBase64(fileItem={FileItem})", fileItem);
        using (var reader = new StreamReader(fileItem.Data))
        {
            using (var memStream = new MemoryStream())
            {
                await reader.BaseStream.CopyToAsync(memStream);
                var fileData = memStream.ToArray();
                var encodedBase64 = Convert.ToBase64String(fileData);

				_logger.LogDebug("<<< Exiting DataToBase64(fileItem={FileItem})", fileItem);
                return encodedBase64;
            }
        }
    }

    private static bool IsExtensionValid(string fileName)
    {
		_logger.LogDebug(">>> Entering IsExtensionValid(fileName={FileName})", fileName);
        var extension = Path.GetExtension(fileName);

		_logger.LogDebug("IsExtensionValid(fileName={FileName}): extension → {Extension}", fileName, extension);
		_logger.LogDebug("<<< Exiting IsExtensionValid(fileName={FileName})", fileName);
        return string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);
    }
}
