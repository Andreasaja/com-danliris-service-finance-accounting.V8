using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class EnhanceVBRealization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "VBRealizationDocumentUnitCostsItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VBNonPoType",
                table: "VBRealizationDocuments",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VBRequestDocumentAmount",
                table: "VBRealizationDocuments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "VBRequestDocumentCreatedBy",
                table: "VBRealizationDocuments",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "VBRealizationDocumentUnitCostsItems");

            migrationBuilder.DropColumn(
                name: "VBNonPoType",
                table: "VBRealizationDocuments");

            migrationBuilder.DropColumn(
                name: "VBRequestDocumentAmount",
                table: "VBRealizationDocuments");

            migrationBuilder.DropColumn(
                name: "VBRequestDocumentCreatedBy",
                table: "VBRealizationDocuments");
        }
    }
}
