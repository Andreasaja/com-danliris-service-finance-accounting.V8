using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class IncomeTaxCreditorAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalPurchaseOrderNo",
                table: "CreditorAccounts",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "IncomeTaxAmount",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxNo",
                table: "CreditorAccounts",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VATAmount",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalPurchaseOrderNo",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "IncomeTaxAmount",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "IncomeTaxNo",
                table: "CreditorAccounts");

            migrationBuilder.DropColumn(
                name: "VATAmount",
                table: "CreditorAccounts");
        }
    }
}
