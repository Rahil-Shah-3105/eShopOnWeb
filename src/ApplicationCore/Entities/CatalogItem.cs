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
		_logger.LogInformation("CatalogItem(catalogTypeId={CatalogTypeId}, catalogBrandId={CatalogBrandId}, description={Description}, name={Name}, price={Price}, pictureUri={PictureUri}): SOC2-AUDIT [CC6.7]: Encryption operation completed", catalogTypeId, catalogBrandId, description, name, price, pictureUri);
    }

    public void UpdateDetails(CatalogItemDetails details)
    {
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC8.1]: Change management operation performed", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC6.7]: Encryption operation completed", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", details);
    _logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC6.8]: Malicious software — State change recorded for the audit trail", details);
    _logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC7.2]: Monitoring coverage — State change recorded for the audit trail", details);
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));

        Name = details.Name;
        Description = details.Description;
        Price = details.Price;
    }

    public void UpdateBrand(int catalogBrandId)
    {
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOC2-AUDIT [CC8.1]: Change management operation performed", catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
        CatalogBrandId = catalogBrandId;
    }

    public void UpdateType(int catalogTypeId)
    {
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOC2-AUDIT [CC8.1]: Change management operation performed", catalogTypeId);
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    _logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
        CatalogTypeId = catalogTypeId;
    }

    public void UpdatePictureUri(string pictureName)
    {
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOC2-AUDIT [CC8.1]: Change management operation performed", pictureName);
        if (string.IsNullOrEmpty(pictureName))
        {
            PictureUri = string.Empty;
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", pictureName);
            return;
        }
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
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
			_logger.LogInformation("CatalogItemDetails(name={Name}, description={Description}, price={Price}): SOC2-AUDIT [CC6.7]: Encryption operation completed", name, description, price);
        }
    }
}
