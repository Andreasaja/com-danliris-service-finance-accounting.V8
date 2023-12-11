using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class MoveColumnToLayer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conversion",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "DealQuantity",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "DealUOMId",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "DealUOMUnit",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "DefaultQuantity",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "DefaultUOMId",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "DefaultUOMUnit",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.DropColumn(
                name: "UseVat",
                table: "VBRequestDocumentEPODetails");

            migrationBuilder.RenameColumn(
                name: "EPONo",
                table: "VBRequestDocumentItems",
                newName: "ProductCode");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "VBRequestDocumentEPODetails",
                newName: "VBRequestDocumentId");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "VBRequestDocumentEPODetails",
                newName: "EPONo");

            migrationBuilder.AddColumn<double>(
                name: "Conversion",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DealQuantity",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DealUOMId",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DealUOMUnit",
                table: "VBRequestDocumentItems",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DefaultQuantity",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DefaultUOMId",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DefaultUOMUnit",
                table: "VBRequestDocumentItems",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "VBRequestDocumentItems",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseVat",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VBRequestDocumentEPODetailId",
                table: "VBRequestDocumentItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conversion",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "DealQuantity",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "DealUOMId",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "DealUOMUnit",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "DefaultQuantity",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "DefaultUOMId",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "DefaultUOMUnit",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "UseVat",
                table: "VBRequestDocumentItems");

            migrationBuilder.DropColumn(
                name: "VBRequestDocumentEPODetailId",
                table: "VBRequestDocumentItems");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "VBRequestDocumentItems",
                newName: "EPONo");

            migrationBuilder.RenameColumn(
                name: "VBRequestDocumentId",
                table: "VBRequestDocumentEPODetails",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "EPONo",
                table: "VBRequestDocumentEPODetails",
                newName: "ProductCode");

            migrationBuilder.AddColumn<double>(
                name: "Conversion",
                table: "VBRequestDocumentEPODetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DealQuantity",
                table: "VBRequestDocumentEPODetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DealUOMId",
                table: "VBRequestDocumentEPODetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DealUOMUnit",
                table: "VBRequestDocumentEPODetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DefaultQuantity",
                table: "VBRequestDocumentEPODetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DefaultUOMId",
                table: "VBRequestDocumentEPODetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DefaultUOMUnit",
                table: "VBRequestDocumentEPODetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "VBRequestDocumentEPODetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "VBRequestDocumentEPODetails",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseVat",
                table: "VBRequestDocumentEPODetails",
                nullable: false,
                defaultValue: false);
        }
    }
}
