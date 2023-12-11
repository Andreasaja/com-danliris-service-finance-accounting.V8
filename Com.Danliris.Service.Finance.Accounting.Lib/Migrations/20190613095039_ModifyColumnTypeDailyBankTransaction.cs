using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class ModifyColumnTypeDailyBankTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountBankId",
                table: "DailyBankTransactions",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountBankCurrencyId",
                table: "DailyBankTransactions",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountBankId",
                table: "BankTransactionMonthlyBalances",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SupplierId",
                table: "DailyBankTransactions",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "DailyBankTransactions",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "AccountBankId",
                table: "DailyBankTransactions",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "AccountBankCurrencyId",
                table: "DailyBankTransactions",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "AccountBankId",
                table: "BankTransactionMonthlyBalances",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 50);
        }
    }
}
