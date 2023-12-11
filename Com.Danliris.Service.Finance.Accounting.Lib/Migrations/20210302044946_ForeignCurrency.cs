using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class ForeignCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrencyBankExpenditureNoteInvoiceAmount",
                table: "GarmentDebtBalances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyIncomeTaxAmount",
                table: "GarmentDebtBalances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyVATAmount",
                table: "GarmentDebtBalances",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyBankExpenditureNoteInvoiceAmount",
                table: "GarmentDebtBalances");

            migrationBuilder.DropColumn(
                name: "CurrencyIncomeTaxAmount",
                table: "GarmentDebtBalances");

            migrationBuilder.DropColumn(
                name: "CurrencyVATAmount",
                table: "GarmentDebtBalances");
        }
    }
}
