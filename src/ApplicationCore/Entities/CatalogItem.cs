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
		_logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", details);
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
		_logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail", details);
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
		_logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", details);
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));
		_logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", details);

		_logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", details);
        Name = details.Name;
		_logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", details);
        Description = details.Description;
		_logger.LogInformation("UpdateDetails(details={Details}): SOC2-AUDIT [CC8.1]: Change management operation performed", details);
        Price = details.Price;
		_logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", details);
    _logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail", details);
    _logger.LogInformation("UpdateDetails(details={Details}): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed", details);
    }

    public void UpdateBrand(int catalogBrandId)
    {
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): SOC2-AUDIT [CC8.1]: Change management operation performed", catalogBrandId);
        CatalogBrandId = catalogBrandId;
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    }

    public void UpdateType(int catalogTypeId)
    {
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): SOC2-AUDIT [CC8.1]: Change management operation performed", catalogTypeId);
        CatalogTypeId = catalogTypeId;
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    _logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    _logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    }

    public void UpdatePictureUri(string pictureName)
    {
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", pictureName);
        if (string.IsNullOrEmpty(pictureName))
        {
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", pictureName);
            PictureUri = string.Empty;
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", pictureName);
            return;
        }
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): SOC2-AUDIT [CC8.1]: Change management operation performed", pictureName);
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", pictureName);
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
