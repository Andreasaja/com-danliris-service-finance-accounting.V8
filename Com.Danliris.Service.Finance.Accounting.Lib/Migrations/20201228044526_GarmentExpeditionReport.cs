using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class GarmentExpeditionReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentDueDays",
                table: "GarmentPurchasingExpeditions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "GarmentPurchasingExpeditions",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "GarmentPurchasingExpeditions",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDueDays",
                table: "GarmentPurchasingExpeditions");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "GarmentPurchasingExpeditions");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "GarmentPurchasingExpeditions");
        }
    }
}
