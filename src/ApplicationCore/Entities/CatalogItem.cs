using System;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Entities;

public class CatalogItem : BaseEntity, IAggregateRoot
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string PictureUri { get; private set; }
    public int CatalogTypeId { get; private set; }
    public CatalogType? CatalogType { get; private set; }
    public int CatalogBrandId { get; private set; }
    public CatalogBrand? CatalogBrand { get; private set; }

    public CatalogItem(int catalogTypeId,
        int catalogBrandId,
        string description,
        string name,
        decimal price,
        string pictureUri)
    {
        CatalogTypeId = catalogTypeId;
        CatalogBrandId = catalogBrandId;
        Description = description;
        Name = name;
        Price = price;
        PictureUri = pictureUri;
		_logger.LogWarning("CatalogItem(catalogTypeId,catalogBrandId,description,name,price,pictureUri)={CatalogTypeId},{CatalogBrandId},{Description},{Name},{Price},{PictureUri}: PCI-AUDIT: Security incident detected", catalogTypeId, catalogBrandId, description, name, price, pictureUri);
    }

    public void UpdateDetails(CatalogItemDetails details)
    {
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));

        Name = details.Name;
        Description = details.Description;
		_logger.LogInformation("UpdateDetails(details)={Details}): PCI-AUDIT: Change management operation performed", details);
        Price = details.Price;
		_logger.LogWarning("UpdateDetails(details)={Details}): PCI-AUDIT: State change recorded for the audit trail", details);
    }

    public void UpdateBrand(int catalogBrandId)
    {
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
		_logger.LogInformation("UpdateBrand(catalogBrandId)={CatalogBrandId}): PCI-AUDIT: Change management operation performed", catalogBrandId);
        CatalogBrandId = catalogBrandId;
		_logger.LogWarning("UpdateBrand(catalogBrandId)={CatalogBrandId}): PCI-AUDIT: State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    }

    public void UpdateType(int catalogTypeId)
    {
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
		_logger.LogInformation("UpdateType(catalogTypeId)={CatalogTypeId}): PCI-AUDIT: Change management operation performed", catalogTypeId);
        CatalogTypeId = catalogTypeId;
		_logger.LogWarning("UpdateType(catalogTypeId)={CatalogTypeId}): PCI-AUDIT: State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    }

    public void UpdatePictureUri(string pictureName)
    {
        if (string.IsNullOrEmpty(pictureName))
        {
            PictureUri = string.Empty;
            return;
        }
		_logger.LogInformation("UpdatePictureUri(pictureName)={PictureName}): PCI-AUDIT: Change management operation performed", pictureName);
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
		_logger.LogWarning("UpdatePictureUri(pictureName)={PictureName}): PCI-AUDIT: State change recorded for the audit trail", pictureName);
    }

    public readonly record struct CatalogItemDetails
    {
    	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
        public string? Name { get; }
        public string? Description { get; }
        public decimal Price { get; }

        public CatalogItemDetails(string? name, string? description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
			_logger.LogWarning("CatalogItemDetails(name,description,price)={Name},{Description},{Price}: PCI-AUDIT: Security incident detected", name, description, price);
        }
    }
}
