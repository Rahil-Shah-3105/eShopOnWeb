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
		_logger.LogDebug(">>> Entering CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri})", catalogTypeId, catalogBrandId, description, name, price, pictureUri);
        CatalogTypeId = catalogTypeId;
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): CatalogTypeId → {CatalogTypeId1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, CatalogTypeId);
        CatalogBrandId = catalogBrandId;
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): CatalogBrandId → {CatalogBrandId1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, CatalogBrandId);
        Description = description;
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): Description → {Description1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, Description);
        Name = name;
        Price = price;
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): Name → {Name1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, Name);
		_logger.LogDebug("CatalogItem(catalogTypeId={CatalogTypeId},catalogBrandId={CatalogBrandId},description={Description},name={Name},price={Price},pictureUri={PictureUri}): Price → {Price1}", catalogTypeId, catalogBrandId, description, name, price, pictureUri, Price);
        PictureUri = pictureUri;
    }

    public void UpdateDetails(CatalogItemDetails details)
    {
		_logger.LogDebug(">>> Entering UpdateDetails(details={Details})", details);
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));
		_logger.LogDebug("<<< Exiting UpdateDetails(details={Details})", details);
		_logger.LogDebug("UpdateDetails(details={Details}): Price → {Price}", details, Price);

        Name = details.Name;
        Description = details.Description;
		_logger.LogDebug("UpdateDetails(details={Details}): Name → {Name}", details, Name);
		_logger.LogDebug("UpdateDetails(details={Details}): Description → {Description}", details, Description);
        Price = details.Price;
    }

    public void UpdateBrand(int catalogBrandId)
    {
		_logger.LogDebug(">>> Entering UpdateBrand(catalogBrandId={CatalogBrandId})", catalogBrandId);
		_logger.LogDebug("UpdateBrand(catalogBrandId={CatalogBrandId}): CatalogBrandId → {CatalogBrandId1}", catalogBrandId, CatalogBrandId);
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
		_logger.LogDebug("<<< Exiting UpdateBrand(catalogBrandId={CatalogBrandId})", catalogBrandId);
        CatalogBrandId = catalogBrandId;
    }

    public void UpdateType(int catalogTypeId)
    {
		_logger.LogDebug(">>> Entering UpdateType(catalogTypeId={CatalogTypeId})", catalogTypeId);
		_logger.LogDebug("UpdateType(catalogTypeId={CatalogTypeId}): CatalogTypeId → {CatalogTypeId1}", catalogTypeId, CatalogTypeId);
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
		_logger.LogDebug("<<< Exiting UpdateType(catalogTypeId={CatalogTypeId})", catalogTypeId);
        CatalogTypeId = catalogTypeId;
    }

    public void UpdatePictureUri(string pictureName)
    {
		_logger.LogDebug(">>> Entering UpdatePictureUri(pictureName={PictureName})", pictureName);
        if (string.IsNullOrEmpty(pictureName))
        {
            PictureUri = string.Empty;
			_logger.LogDebug("UpdatePictureUri(pictureName={PictureName}): PictureUri → {PictureUri}", pictureName, PictureUri);
            return;
        }
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
		_logger.LogDebug("<<< Exiting UpdatePictureUri(pictureName={PictureName})", pictureName);
    }

    public readonly record struct CatalogItemDetails
    {
        public string? Name { get; }
        public string? Description { get; }
        public decimal Price { get; }

        public CatalogItemDetails(string? name, string? description, decimal price)
        {
			_logger.LogDebug(">>> Entering CatalogItemDetails(name={Name},description={Description},price={Price})", name, description, price);
			_logger.LogDebug("CatalogItemDetails(name={Name},description={Description},price={Price}): Price → {Price1}", name, description, price, Price);
			_logger.LogDebug("CatalogItemDetails(name={Name},description={Description},price={Price}): Description → {Description1}", name, description, price, Description);
            Name = name;
            Description = description;
			_logger.LogDebug("CatalogItemDetails(name={Name},description={Description},price={Price}): Name → {Name1}", name, description, price, Name);
            Price = price;
        }
    }
}
