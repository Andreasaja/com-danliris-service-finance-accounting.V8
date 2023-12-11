using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class add_uomId_PurchasingDisposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uom",
                table: "PurchasingDispositionExpeditionItems",
                newName: "UomUnit");

            migrationBuilder.AddColumn<string>(
                name: "UomId",
                table: "PurchasingDispositionExpeditionItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UomId",
                table: "PurchasingDispositionExpeditionItems");

            migrationBuilder.RenameColumn(
                name: "UomUnit",
                table: "PurchasingDispositionExpeditionItems",
                newName: "Uom");
        }
    }
}
