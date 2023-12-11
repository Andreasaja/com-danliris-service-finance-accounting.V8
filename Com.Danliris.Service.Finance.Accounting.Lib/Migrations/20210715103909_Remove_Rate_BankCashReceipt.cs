using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Remove_Rate_BankCashReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "BankCashReceiptModel");

            migrationBuilder.RenameColumn(
                name: "AccUnitCode",
                table: "BankCashReceiptItemModel",
                newName: "AccUnitCoaCode");

            migrationBuilder.RenameColumn(
                name: "AccSubCode",
                table: "BankCashReceiptItemModel",
                newName: "AccSubCoaCode");

            migrationBuilder.RenameColumn(
                name: "AccAmountCode",
                table: "BankCashReceiptItemModel",
                newName: "AccAmountCoaCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccUnitCoaCode",
                table: "BankCashReceiptItemModel",
                newName: "AccUnitCode");

            migrationBuilder.RenameColumn(
                name: "AccSubCoaCode",
                table: "BankCashReceiptItemModel",
                newName: "AccSubCode");

            migrationBuilder.RenameColumn(
                name: "AccAmountCoaCode",
                table: "BankCashReceiptItemModel",
                newName: "AccAmountCode");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "BankCashReceiptModel",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
