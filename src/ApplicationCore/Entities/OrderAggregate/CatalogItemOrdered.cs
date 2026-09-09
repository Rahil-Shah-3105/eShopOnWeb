using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

/// <summary>
/// Represents a snapshot of the item that was ordered. If catalog item details change, details of
/// the item that was part of a completed order should not change.
/// </summary>
public class CatalogItemOrdered // ValueObject
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public CatalogItemOrdered(int catalogItemId, string productName, string pictureUri)
    {
        Guard.Against.OutOfRange(catalogItemId, nameof(catalogItemId), 1, int.MaxValue);
        Guard.Against.NullOrEmpty(productName, nameof(productName));
        Guard.Against.NullOrEmpty(pictureUri, nameof(pictureUri));

        CatalogItemId = catalogItemId;
        ProductName = productName;
        PictureUri = pictureUri;
		_logger.LogWarning("CatalogItemOrdered(catalogItemId,productName,pictureUri)={CatalogItemId},{ProductName},{PictureUri}: M2614-AUDIT: Security incident detected", catalogItemId, productName, pictureUri);
    }

    #pragma warning disable CS8618 // Required by Entity Framework
    private CatalogItemOrdered() {}

    public int CatalogItemId { get; private set; }
    public string ProductName { get; private set; }
    public string PictureUri { get; private set; }
}
