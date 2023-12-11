using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddSupplierIsImportInCreditorAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EPONo",
                table: "PurchasingDispositionExpeditionItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupplierIsImport",
                table: "CreditorAccounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPONo",
                table: "PurchasingDispositionExpeditionItems");

            migrationBuilder.DropColumn(
                name: "SupplierIsImport",
                table: "CreditorAccounts");
        }
    }
}
