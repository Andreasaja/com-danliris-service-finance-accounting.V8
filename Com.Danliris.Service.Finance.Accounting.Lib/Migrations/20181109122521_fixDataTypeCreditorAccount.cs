using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class fixDataTypeCreditorAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "UnitReceiptNotePPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "UnitReceiptNoteDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "UnitReceiptMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "MemoPPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "MemoMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "MemoDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "FinalBalance",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "BankExpenditureNotePPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "BankExpenditureNoteMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "BankExpenditureNoteDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UnitReceiptNotePPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "UnitReceiptNoteDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "UnitReceiptMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "MemoPPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "MemoMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "MemoDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "FinalBalance",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "BankExpenditureNotePPN",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "BankExpenditureNoteMutation",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "BankExpenditureNoteDPP",
                table: "CreditorAccounts",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
