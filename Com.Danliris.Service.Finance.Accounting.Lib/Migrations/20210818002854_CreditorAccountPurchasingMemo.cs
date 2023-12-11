using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class CreditorAccountPurchasingMemo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PurchasingMemoAmount",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PurchasingMemoId",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PurchasingMemoNo",
                table: "CreditorAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasingMemoAmount",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "PurchasingMemoId",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "PurchasingMemoNo",
                table: "CreditorAccounts");
        }
    }
}
