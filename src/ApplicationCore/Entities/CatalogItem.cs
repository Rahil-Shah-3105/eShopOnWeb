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
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));

		_logger.LogInformation("UpdateDetails(details={Details}): DORA-AUDIT: Third party risk — Change management operation performed", details);
        Name = details.Name;
		_logger.LogInformation("UpdateDetails(details={Details}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail", details);
        Description = details.Description;
		_logger.LogInformation("UpdateDetails(details={Details}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail", details);
        Price = details.Price;
		_logger.LogInformation("UpdateDetails(details={Details}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail", details);
    }

    public void UpdateBrand(int catalogBrandId)
    {
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
        CatalogBrandId = catalogBrandId;
		_logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail {CatalogBrandId1}", catalogBrandId, catalogBrandId);
    _logger.LogInformation("UpdateBrand(catalogBrandId={CatalogBrandId}): DORA-AUDIT: Third party risk — Change management operation performed", catalogBrandId);
    }

    public void UpdateType(int catalogTypeId)
    {
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
        CatalogTypeId = catalogTypeId;
		_logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail {CatalogTypeId1}", catalogTypeId, catalogTypeId);
    _logger.LogInformation("UpdateType(catalogTypeId={CatalogTypeId}): DORA-AUDIT: Third party risk — Change management operation performed", catalogTypeId);
    }

    public void UpdatePictureUri(string pictureName)
    {
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): DORA-AUDIT: Third party risk — Change management operation performed", pictureName);
        if (string.IsNullOrEmpty(pictureName))
        {
			_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): DORA-AUDIT: Resilience testing — State change recorded for the audit trail", pictureName);
            PictureUri = string.Empty;
            return;
        }
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): DORA-AUDIT [Art. 45]: Information sharing — State change recorded for the audit trail", pictureName);
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
		_logger.LogInformation("UpdatePictureUri(pictureName={PictureName}): DORA-AUDIT: Incident coverage — State change recorded for the audit trail", pictureName);
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
