using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class CreditorAccountEnhancement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitReceiptNotePPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitReceiptNoteDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitReceiptMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "MemoPPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "MemoMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "MemoDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalBalance",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "BankExpenditureNotePPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "BankExpenditureNoteMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "BankExpenditureNoteDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<decimal>(
                name: "MemoDPPCurrency",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MemoPaymentDuration",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemoDPPCurrency",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "MemoPaymentDuration",
                table: "CreditorAccounts");

            migrationBuilder.AlterColumn<double>(
                name: "UnitReceiptNotePPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "UnitReceiptNoteDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "UnitReceiptMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "MemoPPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "MemoMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "MemoDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "FinalBalance",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "BankExpenditureNotePPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "BankExpenditureNoteMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "BankExpenditureNoteDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
