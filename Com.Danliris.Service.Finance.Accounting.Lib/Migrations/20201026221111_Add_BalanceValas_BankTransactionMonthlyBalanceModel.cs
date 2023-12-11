using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_BalanceValas_BankTransactionMonthlyBalanceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "InitialBalanceValas",
                table: "BankTransactionMonthlyBalances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RemainingBalanceValas",
                table: "BankTransactionMonthlyBalances",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialBalanceValas",
                table: "BankTransactionMonthlyBalances");

            migrationBuilder.DropColumn(
                name: "RemainingBalanceValas",
                table: "BankTransactionMonthlyBalances");
        }
    }
}
