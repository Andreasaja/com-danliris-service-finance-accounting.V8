using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Updating_PurchasingDispositionExpeditionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "PurchasingDispositionExpeditions",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DPP",
                table: "PurchasingDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                table: "PurchasingDispositionExpeditions",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionId",
                table: "PurchasingDispositionExpeditions",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "PurchasingDispositionExpeditions",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncomeTaxValue",
                table: "PurchasingDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "SupplierId",
                table: "PurchasingDispositionExpeditions",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VatValue",
                table: "PurchasingDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "DPP",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "DivisionCode",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "IncomeTaxValue",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "VatValue",
                table: "PurchasingDispositionExpeditions");
        }
    }
}
