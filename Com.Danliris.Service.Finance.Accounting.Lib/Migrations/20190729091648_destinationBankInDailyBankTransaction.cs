using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class destinationBankInDailyBankTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationBankAccountName",
                table: "DailyBankTransactions",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationBankAccountNumber",
                table: "DailyBankTransactions",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationBankCode",
                table: "DailyBankTransactions",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationBankCurrencyCode",
                table: "DailyBankTransactions",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestinationBankCurrencyId",
                table: "DailyBankTransactions",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DestinationBankCurrencySymbol",
                table: "DailyBankTransactions",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestinationBankId",
                table: "DailyBankTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DestinationBankName",
                table: "DailyBankTransactions",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationBankAccountName",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationBankAccountNumber",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationBankCode",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationBankCurrencyCode",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationBankCurrencyId",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationBankCurrencySymbol",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationBankId",
                table: "DailyBankTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationBankName",
                table: "DailyBankTransactions");
        }
    }
}
