using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class BankDailyBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Nominal",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "BeforeNominal",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "AfterNominal",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<decimal>(
                name: "NominalOut",
                table: "DailyBankTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "DailyBankTransactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NominalOut",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "DailyBankTransactions");

            migrationBuilder.AlterColumn<double>(
                name: "Nominal",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "BeforeNominal",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "AfterNominal",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
