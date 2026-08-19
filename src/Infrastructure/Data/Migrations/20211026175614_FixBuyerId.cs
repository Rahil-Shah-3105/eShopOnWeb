using Microsoft.Extensions.Logging;
﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.eShopWeb.Infrastructure.Data.Migrations;

public partial class FixBuyerId : Migration
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    protected override void Up(MigrationBuilder migrationBuilder)
    {
		_logger.LogDebug(">>> Entering Up(migrationBuilder={MigrationBuilder})", migrationBuilder);
        migrationBuilder.AlterColumn<string>(
            name: "BuyerId",
            table: "Orders",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "BuyerId",
            table: "Baskets",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(40)",
            oldMaxLength: 40);
			_logger.LogDebug("<<< Exiting Up(migrationBuilder={MigrationBuilder})", migrationBuilder);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
		_logger.LogDebug(">>> Entering Down(migrationBuilder={MigrationBuilder})", migrationBuilder);
        migrationBuilder.AlterColumn<string>(
            name: "BuyerId",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(256)",
            oldMaxLength: 256);

        migrationBuilder.AlterColumn<string>(
            name: "BuyerId",
            table: "Baskets",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(256)",
            oldMaxLength: 256);
			_logger.LogDebug("<<< Exiting Down(migrationBuilder={MigrationBuilder})", migrationBuilder);
    }
}
