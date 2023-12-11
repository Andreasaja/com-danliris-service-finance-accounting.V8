using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Update_BankCashReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankCashReceiptTypeCoaCode",
                table: "GarmentFinanceBankCashReceipts",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankCashReceiptTypeCoaId",
                table: "GarmentFinanceBankCashReceipts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BankCashReceiptTypeId",
                table: "GarmentFinanceBankCashReceipts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BankCashReceiptTypeName",
                table: "GarmentFinanceBankCashReceipts",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "GarmentFinanceBankCashReceipts",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "GarmentFinanceBankCashReceipts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "GarmentFinanceBankCashReceipts",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCashReceiptTypeCoaCode",
                table: "GarmentFinanceBankCashReceipts");

            migrationBuilder.DropColumn(
                name: "BankCashReceiptTypeCoaId",
                table: "GarmentFinanceBankCashReceipts");

            migrationBuilder.DropColumn(
                name: "BankCashReceiptTypeId",
                table: "GarmentFinanceBankCashReceipts");

            migrationBuilder.DropColumn(
                name: "BankCashReceiptTypeName",
                table: "GarmentFinanceBankCashReceipts");

            migrationBuilder.DropColumn(
                name: "BuyerCode",
                table: "GarmentFinanceBankCashReceipts");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "GarmentFinanceBankCashReceipts");

            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "GarmentFinanceBankCashReceipts");
        }
    }
}
