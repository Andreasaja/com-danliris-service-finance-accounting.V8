using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Update_Table_MemoDetailGarmentPurchasings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountingBookId",
                table: "MemoDetailGarmentPurchasings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccountingBookType",
                table: "MemoDetailGarmentPurchasings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GarmentCurrenciesCode",
                table: "MemoDetailGarmentPurchasings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GarmentCurrenciesId",
                table: "MemoDetailGarmentPurchasings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GarmentCurrenciesRate",
                table: "MemoDetailGarmentPurchasings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "MemoDate",
                table: "MemoDetailGarmentPurchasings",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "MemoNo",
                table: "MemoDetailGarmentPurchasings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountingBookId",
                table: "MemoDetailGarmentPurchasings");

            migrationBuilder.DropColumn(
                name: "AccountingBookType",
                table: "MemoDetailGarmentPurchasings");

            migrationBuilder.DropColumn(
                name: "GarmentCurrenciesCode",
                table: "MemoDetailGarmentPurchasings");

            migrationBuilder.DropColumn(
                name: "GarmentCurrenciesId",
                table: "MemoDetailGarmentPurchasings");

            migrationBuilder.DropColumn(
                name: "GarmentCurrenciesRate",
                table: "MemoDetailGarmentPurchasings");

            migrationBuilder.DropColumn(
                name: "MemoDate",
                table: "MemoDetailGarmentPurchasings");

            migrationBuilder.DropColumn(
                name: "MemoNo",
                table: "MemoDetailGarmentPurchasings");
        }
    }
}
