using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class CreditorAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemoDPPCurrency",
                table: "CreditorAccounts",
                newName: "DPPCurrency");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrencyRate",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                table: "CreditorAccounts");

            migrationBuilder.RenameColumn(
                name: "DPPCurrency",
                table: "CreditorAccounts",
                newName: "MemoDPPCurrency");
        }
    }
}
