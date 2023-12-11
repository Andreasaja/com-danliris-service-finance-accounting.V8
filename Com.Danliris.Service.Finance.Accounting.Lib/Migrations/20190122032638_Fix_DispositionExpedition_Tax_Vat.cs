using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Fix_DispositionExpedition_Tax_Vat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeTax",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "PurchasingDispositionExpeditions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "IncomeTax",
                table: "PurchasingDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Vat",
                table: "PurchasingDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
