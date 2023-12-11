using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UnitPaymentOrderInfoColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Date",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxBy",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeTaxId",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxName",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncomeTaxRate",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseIncomeTax",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseVat",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VBRealizationDocumentExpenditureItemId",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitPaymentOrderId",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitPaymentOrderNo",
                table: "VBRealizationDocumentExpenditureItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "IncomeTaxBy",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "IncomeTaxId",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "IncomeTaxName",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "IncomeTaxRate",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "UseIncomeTax",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "UseVat",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "VBRealizationDocumentExpenditureItemId",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "UnitPaymentOrderId",
                table: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropColumn(
                name: "UnitPaymentOrderNo",
                table: "VBRealizationDocumentExpenditureItems");
        }
    }
}
