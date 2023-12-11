using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class enhance_bankCharge_JournalTrnsactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BankCharges",
                table: "DailyBankTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Rates",
                table: "DailyBankTransactions",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCharges",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "Rates",
                table: "DailyBankTransactions");
        }
    }
}
