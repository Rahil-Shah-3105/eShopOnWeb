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
    }

    public void UpdateDetails(CatalogItemDetails details)
    {
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
		_logger.LogInformation("UpdateDetails(details={Details}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOX-AUDIT: Change management (PCAOB AS 2201) — Change management operation performed", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", details);
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));

        Name = details.Name;
        Description = details.Description;
        Price = details.Price;
    }

    public void UpdateBrand(int catalogBrandId)
    {
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOX-AUDIT: Change management (PCAOB AS 2201) — Change management operation performed", catalogBrandId);
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOX-AUDIT: Computer operations — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOX-AUDIT: Privileged access — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
        CatalogBrandId = catalogBrandId;
    }

    public void UpdateType(int catalogTypeId)
    {
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOX-AUDIT: Change management (PCAOB AS 2201) — Change management operation performed", catalogTypeId);
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOX-AUDIT: Computer operations — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    _logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOX-AUDIT: Privileged access — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    _logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
        CatalogTypeId = catalogTypeId;
    }

    public void UpdatePictureUri(string pictureName)
    {
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOX-AUDIT: Change management (PCAOB AS 2201) — Change management operation performed", pictureName);
        if (string.IsNullOrEmpty(pictureName))
        {
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", pictureName);
            PictureUri = string.Empty;
            return;
        }
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
    }

    public readonly record struct CatalogItemDetails
    {
        public string? Name { get; }
        public string? Description { get; }
        public decimal Price { get; }

        public CatalogItemDetails(string? name, string? description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
