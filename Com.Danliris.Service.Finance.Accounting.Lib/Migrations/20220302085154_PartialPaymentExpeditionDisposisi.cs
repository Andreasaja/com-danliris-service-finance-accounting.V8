using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class PartialPaymentExpeditionDisposisi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountPaid",
                table: "PurchasingDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SupplierPayment",
                table: "PurchasingDispositionExpeditions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "PurchasingDispositionExpeditions");

            migrationBuilder.DropColumn(
                name: "SupplierPayment",
                table: "PurchasingDispositionExpeditions");
        }
    }
}
