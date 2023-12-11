using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class BankCurrencyDPPVATBankExpenditureNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankCurrencyCode",
                table: "DPPVATBankExpenditureNotes",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankCurrencyId",
                table: "DPPVATBankExpenditureNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "BankCurrencyRate",
                table: "DPPVATBankExpenditureNotes",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCurrencyCode",
                table: "DPPVATBankExpenditureNotes");

            migrationBuilder.DropColumn(
                name: "BankCurrencyId",
                table: "DPPVATBankExpenditureNotes");

            migrationBuilder.DropColumn(
                name: "BankCurrencyRate",
                table: "DPPVATBankExpenditureNotes");
        }
    }
}
