using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data.Migrations;

public partial class InitialModel : Migration
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    protected override void Up(MigrationBuilder migrationBuilder)
    {
		_logger.LogDebug(">>> Entering Up(migrationBuilder={MigrationBuilder})", migrationBuilder);
        migrationBuilder.CreateSequence(
            name: "catalog_brand_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "catalog_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "catalog_type_hilo",
            incrementBy: 10);

        migrationBuilder.CreateTable(
            name: "Baskets",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                BuyerId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): BuyerId → {BuyerId}", migrationBuilder, BuyerId);
                table.PrimaryKey("PK_Baskets", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CatalogBrands",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false),
                Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): Brand → {Brand}", migrationBuilder, Brand);
                table.PrimaryKey("PK_CatalogBrands", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CatalogTypes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false),
                Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): Type → {Type}", migrationBuilder, Type);
                table.PrimaryKey("PK_CatalogTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                BuyerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ShipToAddress_Street = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: true),
                ShipToAddress_City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ShipToAddress_State = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                ShipToAddress_Country = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                ShipToAddress_ZipCode = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true)
            },
            constraints: table =>
            {
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): ShipToAddress_Country → {ShipToAddress_Country}", migrationBuilder, ShipToAddress_Country);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): ShipToAddress_ZipCode → {ShipToAddress_ZipCode}", migrationBuilder, ShipToAddress_ZipCode);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): ShipToAddress_Street → {ShipToAddress_Street}", migrationBuilder, ShipToAddress_Street);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): ShipToAddress_State → {ShipToAddress_State}", migrationBuilder, ShipToAddress_State);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): OrderDate → {OrderDate}", migrationBuilder, OrderDate);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): ShipToAddress_City → {ShipToAddress_City}", migrationBuilder, ShipToAddress_City);
                table.PrimaryKey("PK_Orders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "BasketItems",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                CatalogItemId = table.Column<int>(type: "int", nullable: false),
                BasketId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): Quantity → {Quantity}", migrationBuilder, Quantity);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): UnitPrice → {UnitPrice}", migrationBuilder, UnitPrice);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): CatalogItemId → {CatalogItemId}", migrationBuilder, CatalogItemId);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): BasketId → {BasketId}", migrationBuilder, BasketId);
                table.PrimaryKey("PK_BasketItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_BasketItems_Baskets_BasketId",
                    column: x => x.BasketId,
                    principalTable: "Baskets",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Catalog",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                PictureUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CatalogTypeId = table.Column<int>(type: "int", nullable: false),
                CatalogBrandId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): Price → {Price}", migrationBuilder, Price);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): CatalogTypeId → {CatalogTypeId}", migrationBuilder, CatalogTypeId);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): CatalogBrandId → {CatalogBrandId}", migrationBuilder, CatalogBrandId);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): PictureUri → {PictureUri}", migrationBuilder, PictureUri);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): Description → {Description}", migrationBuilder, Description);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): Name → {Name}", migrationBuilder, Name);
                table.PrimaryKey("PK_Catalog", x => x.Id);
                table.ForeignKey(
                    name: "FK_Catalog_CatalogBrands_CatalogBrandId",
                    column: x => x.CatalogBrandId,
                    principalTable: "CatalogBrands",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Catalog_CatalogTypes_CatalogTypeId",
                    column: x => x.CatalogTypeId,
                    principalTable: "CatalogTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "OrderItems",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ItemOrdered_CatalogItemId = table.Column<int>(type: "int", nullable: true),
                ItemOrdered_ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ItemOrdered_PictureUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Units = table.Column<int>(type: "int", nullable: false),
                OrderId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): OrderId → {OrderId}", migrationBuilder, OrderId);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): ItemOrdered_CatalogItemId → {ItemOrdered_CatalogItemId}", migrationBuilder, ItemOrdered_CatalogItemId);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): ItemOrdered_ProductName → {ItemOrdered_ProductName}", migrationBuilder, ItemOrdered_ProductName);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): Units → {Units}", migrationBuilder, Units);
				_logger.LogDebug("Up(migrationBuilder={MigrationBuilder}): ItemOrdered_PictureUri → {ItemOrdered_PictureUri}", migrationBuilder, ItemOrdered_PictureUri);
                table.PrimaryKey("PK_OrderItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_OrderItems_Orders_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Orders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_BasketItems_BasketId",
            table: "BasketItems",
            column: "BasketId");

        migrationBuilder.CreateIndex(
            name: "IX_Catalog_CatalogBrandId",
            table: "Catalog",
            column: "CatalogBrandId");

        migrationBuilder.CreateIndex(
            name: "IX_Catalog_CatalogTypeId",
            table: "Catalog",
            column: "CatalogTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_OrderItems_OrderId",
            table: "OrderItems",
            column: "OrderId");
			_logger.LogDebug("<<< Exiting Up(migrationBuilder={MigrationBuilder})", migrationBuilder);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
		_logger.LogDebug(">>> Entering Down(migrationBuilder={MigrationBuilder})", migrationBuilder);
        migrationBuilder.DropTable(
            name: "BasketItems");

        migrationBuilder.DropTable(
            name: "Catalog");

        migrationBuilder.DropTable(
            name: "OrderItems");

        migrationBuilder.DropTable(
            name: "Baskets");

        migrationBuilder.DropTable(
            name: "CatalogBrands");

        migrationBuilder.DropTable(
            name: "CatalogTypes");

        migrationBuilder.DropTable(
            name: "Orders");

        migrationBuilder.DropSequence(
            name: "catalog_brand_hilo");

        migrationBuilder.DropSequence(
            name: "catalog_hilo");

        migrationBuilder.DropSequence(
            name: "catalog_type_hilo");
			_logger.LogDebug("<<< Exiting Down(migrationBuilder={MigrationBuilder})", migrationBuilder);
    }
}
