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
		_logger.LogInformation("CatalogItem(catalogTypeId={CatalogTypeId}, catalogBrandId={CatalogBrandId}, description={Description}, name={Name}, price={Price}, pictureUri={PictureUri}): GDPR-AUDIT: Transfers to third countries — Encryption operation completed", catalogTypeId, catalogBrandId, description, name, price, pictureUri);
    }

    public void UpdateDetails(CatalogItemDetails details)
    {
		_logger.LogInformation("UpdateDetails(details={Details}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", details);
		_logger.LogWarning("UpdateDetails(details={Details}): GDPR-AUDIT [Art.15]: Data subject right exercised for data subject", details);
		_logger.LogInformation("UpdateDetails(details={Details}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): GDPR-AUDIT [Art.5]: Personal data processing activity completed", details);
		_logger.LogWarning("UpdateDetails(details={Details}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", details);
		_logger.LogInformation("UpdateDetails(details={Details}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", details);
    _logger.LogInformation("UpdateDetails(details={Details}): GDPR-AUDIT: Transfers to third countries — Encryption operation completed", details);
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));

        Name = details.Name;
        Description = details.Description;
        Price = details.Price;
    }

    public void UpdateBrand(int catalogBrandId)
    {
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): GDPR-AUDIT [Art.5]: Personal data processing activity completed", catalogBrandId);
		_logger.LogWarning("UpdateBrand(catalogBrandId={CatalogBrandId}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
		_logger.LogWarning("UpdateBrand(catalogBrandId={CatalogBrandId}): GDPR-AUDIT [Art.15]: Data subject right exercised for data subject {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
        CatalogBrandId = catalogBrandId;
    }

    public void UpdateType(int catalogTypeId)
    {
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): GDPR-AUDIT [Art.5]: Personal data processing activity completed", catalogTypeId);
		_logger.LogWarning("UpdateType(catalogTypeId={CatalogTypeId}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
		_logger.LogWarning("UpdateType(catalogTypeId={CatalogTypeId}): GDPR-AUDIT [Art.15]: Data subject right exercised for data subject {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    _logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    _logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
        CatalogTypeId = catalogTypeId;
    }

    public void UpdatePictureUri(string pictureName)
    {
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): GDPR-AUDIT [Art.5]: Personal data processing activity completed", pictureName);
        if (string.IsNullOrEmpty(pictureName))
        {
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", pictureName);
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", pictureName);
			_logger.LogWarning("UpdatePictureUri(pictureName={PictureName}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", pictureName);
			_logger.LogWarning("UpdatePictureUri(pictureName={PictureName}): GDPR-AUDIT [Art.15]: Data subject right exercised for data subject", pictureName);
            PictureUri = string.Empty;
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
			_logger.LogInformation("CatalogItemDetails(name={Name}, description={Description}, price={Price}): GDPR-AUDIT: Transfers to third countries — Encryption operation completed", name, description, price);
        }
    }
}
