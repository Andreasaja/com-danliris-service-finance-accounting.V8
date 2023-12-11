using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class update_BankCashReceiptItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccAmountCoaCode",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "AccAmountCoaId",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "AccAmountCoaName",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "AccUnitCoaCode",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "AccUnitCoaId",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "AccUnitCoaName",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "C1A",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "C1B",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "C2A",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "C2B",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.DropColumn(
                name: "C2C",
                table: "GarmentFinanceBankCashReceiptItems");

            migrationBuilder.AddColumn<string>(
                name: "BankCashReceiptTypeCoaName",
                table: "GarmentFinanceBankCashReceipts",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCashReceiptTypeCoaName",
                table: "GarmentFinanceBankCashReceipts");

            migrationBuilder.AddColumn<string>(
                name: "AccAmountCoaCode",
                table: "GarmentFinanceBankCashReceiptItems",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccAmountCoaId",
                table: "GarmentFinanceBankCashReceiptItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccAmountCoaName",
                table: "GarmentFinanceBankCashReceiptItems",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccUnitCoaCode",
                table: "GarmentFinanceBankCashReceiptItems",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccUnitCoaId",
                table: "GarmentFinanceBankCashReceiptItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccUnitCoaName",
                table: "GarmentFinanceBankCashReceiptItems",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "C1A",
                table: "GarmentFinanceBankCashReceiptItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "C1B",
                table: "GarmentFinanceBankCashReceiptItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "C2A",
                table: "GarmentFinanceBankCashReceiptItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "C2B",
                table: "GarmentFinanceBankCashReceiptItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "C2C",
                table: "GarmentFinanceBankCashReceiptItems",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
