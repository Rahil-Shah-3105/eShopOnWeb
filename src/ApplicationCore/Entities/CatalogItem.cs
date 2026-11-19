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
		_logger.LogError(">>> Entering CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri})", catalogTypeId, catalogBrandId, description, name, price, pictureUri);
        CatalogTypeId = catalogTypeId;
        CatalogBrandId = catalogBrandId;
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): CatalogBrandId → {CatalogBrandId1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, CatalogBrandId);
        Description = description;
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): Description → {Description1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, Description);
        Name = name;
		_logger.LogError("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): Name → {Name1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, Name);
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): Name → {Name1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, Name);
        Price = price;
        PictureUri = pictureUri;
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): PictureUri → {PictureUri1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, PictureUri);
		_logger.LogWarning("<<< Exiting CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri})", catalogTypeId, catalogBrandId, description, name, price, pictureUri);
    }

    public void UpdateDetails(CatalogItemDetails details)
    {
		_logger.LogError(">>> Entering UpdateDetails(details={Details})", details);
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));
		_logger.LogWarning("<<< Exiting UpdateDetails(details={Details})", details);

        Name = details.Name;
		_logger.LogError("UpdateDetails(details={Details}): Name → {Name}", details, Name);
		_logger.LogDebug("UpdateDetails(details={Details}): Name → {Name}", details, Name);
        Description = details.Description;
		_logger.LogDebug("UpdateDetails(details={Details}): Description → {Description}", details, Description);
        Price = details.Price;
		_logger.LogDebug("UpdateDetails(details={Details}): Price → {Price}", details, Price);
    }

    public void UpdateBrand(int catalogBrandId)
    {
		_logger.LogError(">>> Entering UpdateBrand(catalogBrandId={CatalogBrandId})", catalogBrandId);
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
		_logger.LogWarning("<<< Exiting UpdateBrand(catalogBrandId={CatalogBrandId})", catalogBrandId);
        CatalogBrandId = catalogBrandId;
		_logger.LogDebug("UpdateBrand(catalogBrandId={CatalogBrandId}): CatalogBrandId → {CatalogBrandId1}", catalogBrandId, CatalogBrandId);
    }

    public void UpdateType(int catalogTypeId)
    {
		_logger.LogError(">>> Entering UpdateType(catalogTypeId={CatalogTypeId})", catalogTypeId);
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
		_logger.LogWarning("<<< Exiting UpdateType(catalogTypeId={CatalogTypeId})", catalogTypeId);
        CatalogTypeId = catalogTypeId;
		_logger.LogDebug("UpdateType(catalogTypeId={CatalogTypeId}): CatalogTypeId → {CatalogTypeId1}", catalogTypeId, CatalogTypeId);
    }

    public void UpdatePictureUri(string pictureName)
    {
		_logger.LogError(">>> Entering UpdatePictureUri(pictureName={PictureName})", pictureName);
        if (string.IsNullOrEmpty(pictureName))
        {
            PictureUri = string.Empty;
			_logger.LogError("UpdatePictureUri(pictureName={PictureName}): PictureUri → {PictureUri}", pictureName, PictureUri);
			_logger.LogDebug("UpdatePictureUri(pictureName={PictureName}): PictureUri → {PictureUri}", pictureName, PictureUri);
            return;
        }
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
		_logger.LogWarning("<<< Exiting UpdatePictureUri(pictureName={PictureName})", pictureName);
    }

    public readonly record struct CatalogItemDetails
    {
    	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
        public string? Name { get; }
        public string? Description { get; }
        public decimal Price { get; }

        public CatalogItemDetails(string? name, string? description, decimal price)
        {
			_logger.LogError(">>> Entering CatalogItemDetails(name={Name},description={Description},price={Price})", name, description, price);
            Name = name;
            Description = description;
			_logger.LogDebug("CatalogItemDetails(name={Name},description={Description},price={Price}): Name → {Name1}", name, description, price, Name);
			_logger.LogDebug("CatalogItemDetails(name={Name},description={Description},price={Price}): Description → {Description1}", name, description, price, Description);
        _logger.LogWarning("<<< Exiting CatalogItemDetails(name={Name},description={Description},price={Price})", name, description, price);
            Price = price;
			_logger.LogDebug("CatalogItemDetails(name={Name},description={Description},price={Price}): Price → {Price1}", name, description, price, Price);
        }
    }
}
