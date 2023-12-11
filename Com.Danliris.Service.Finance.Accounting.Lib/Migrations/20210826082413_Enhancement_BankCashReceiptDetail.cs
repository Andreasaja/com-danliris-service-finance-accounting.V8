using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Enhancement_BankCashReceiptDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {        
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "GarmentFinanceBankCashReceiptDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DebitCoaCode",
                table: "GarmentFinanceBankCashReceiptDetails",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DebitCoaId",
                table: "GarmentFinanceBankCashReceiptDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DebitCoaName",
                table: "GarmentFinanceBankCashReceiptDetails",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceCoaCode",
                table: "GarmentFinanceBankCashReceiptDetails",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceCoaId",
                table: "GarmentFinanceBankCashReceiptDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceCoaName",
                table: "GarmentFinanceBankCashReceiptDetails",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "GarmentFinanceBankCashReceiptDetailItems",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "GarmentFinanceBankCashReceiptDetails");

            migrationBuilder.DropColumn(
                name: "DebitCoaCode",
                table: "GarmentFinanceBankCashReceiptDetails");

            migrationBuilder.DropColumn(
                name: "DebitCoaId",
                table: "GarmentFinanceBankCashReceiptDetails");

            migrationBuilder.DropColumn(
                name: "DebitCoaName",
                table: "GarmentFinanceBankCashReceiptDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceCoaCode",
                table: "GarmentFinanceBankCashReceiptDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceCoaId",
                table: "GarmentFinanceBankCashReceiptDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceCoaName",
                table: "GarmentFinanceBankCashReceiptDetails");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "GarmentFinanceBankCashReceiptDetailItems");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "GarmentFinanceMemorialDetailItems",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
