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
		_logger.LogWarning("CatalogItem(catalogTypeId,catalogBrandId,description,name,price,pictureUri)={CatalogTypeId},{CatalogBrandId},{Description},{Name},{Price},{PictureUri}: SOC2-AUDIT [CC7.3]: Security incident detected", catalogTypeId, catalogBrandId, description, name, price, pictureUri);
		_logger.LogInformation("CatalogItem(catalogTypeId,catalogBrandId,description,name,price,pictureUri)={CatalogTypeId},{CatalogBrandId},{Description},{Name},{Price},{PictureUri}: SOC2-AUDIT [CC6.7]: Encryption operation completed", catalogTypeId, catalogBrandId, description, name, price, pictureUri);
        PictureUri = pictureUri;
    }

    public void UpdateDetails(CatalogItemDetails details)
    {
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));

        Name = details.Name;
        Description = details.Description;
		_logger.LogInformation("UpdateDetails(details)={Details}): SOC2-AUDIT [CC8.1]: Change management operation performed", details);
		_logger.LogInformation("UpdateDetails(details)={Details}): SOC2-AUDIT [CC6.7]: Encryption operation completed", details);
        Price = details.Price;
    }

    public void UpdateBrand(int catalogBrandId)
    {
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
        CatalogBrandId = catalogBrandId;
		_logger.LogInformation("UpdateBrand(catalogBrandId)={CatalogBrandId}): SOC2-AUDIT [CC8.1]: Change management operation performed", catalogBrandId);
    }

    public void UpdateType(int catalogTypeId)
    {
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
        CatalogTypeId = catalogTypeId;
		_logger.LogInformation("UpdateType(catalogTypeId)={CatalogTypeId}): SOC2-AUDIT [CC8.1]: Change management operation performed", catalogTypeId);
    }

    public void UpdatePictureUri(string pictureName)
    {
        if (string.IsNullOrEmpty(pictureName))
        {
            PictureUri = string.Empty;
            return;
        }
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
		_logger.LogInformation("UpdatePictureUri(pictureName)={PictureName}): SOC2-AUDIT [CC8.1]: Change management operation performed", pictureName);
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
			_logger.LogWarning("CatalogItemDetails(name,description,price)={Name},{Description},{Price}: SOC2-AUDIT [CC7.3]: Security incident detected", name, description, price);
			_logger.LogInformation("CatalogItemDetails(name,description,price)={Name},{Description},{Price}: SOC2-AUDIT [CC6.7]: Encryption operation completed", name, description, price);
            Price = price;
        }
    }
}
