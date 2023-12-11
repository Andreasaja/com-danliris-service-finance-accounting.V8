using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateVBRealization3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountNonPO",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCodeNonPO",
                table: "RealizationVbs",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrencyRateNonPO",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateVB",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "UnitLoad",
                table: "RealizationVbs",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountNonPO",
                table: "RealizationVbDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateNonPO",
                table: "RealizationVbDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "RealizationVbDetails",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isGetPPn",
                table: "RealizationVbDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountNonPO",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "CurrencyCodeNonPO",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "CurrencyRateNonPO",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "DateVB",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "UnitLoad",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "AmountNonPO",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "DateNonPO",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "isGetPPn",
                table: "RealizationVbDetails");
        }
    }
}
