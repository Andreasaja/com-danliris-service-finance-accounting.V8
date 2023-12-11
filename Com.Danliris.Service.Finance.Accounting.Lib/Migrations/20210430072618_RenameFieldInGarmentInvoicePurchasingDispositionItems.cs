using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class RenameFieldInGarmentInvoicePurchasingDispositionItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDispositions_GarmentInvoicePurchasingDisposistionId",
                table: "GarmentInvoicePurchasingDispositionItems");

            migrationBuilder.RenameColumn(
                name: "GarmentInvoicePurchasingDisposistionId",
                table: "GarmentInvoicePurchasingDispositionItems",
                newName: "GarmentInvoicePurchasingDispositionId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDisposistionId",
                table: "GarmentInvoicePurchasingDispositionItems",
                newName: "IX_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDispositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDispositions_GarmentInvoicePurchasingDispositionId",
                table: "GarmentInvoicePurchasingDispositionItems",
                column: "GarmentInvoicePurchasingDispositionId",
                principalTable: "GarmentInvoicePurchasingDispositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDispositions_GarmentInvoicePurchasingDispositionId",
                table: "GarmentInvoicePurchasingDispositionItems");

            migrationBuilder.RenameColumn(
                name: "GarmentInvoicePurchasingDispositionId",
                table: "GarmentInvoicePurchasingDispositionItems",
                newName: "GarmentInvoicePurchasingDisposistionId");

            migrationBuilder.RenameIndex(
                name: "IX_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDispositionId",
                table: "GarmentInvoicePurchasingDispositionItems",
                newName: "IX_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDisposistionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDispositions_GarmentInvoicePurchasingDisposistionId",
                table: "GarmentInvoicePurchasingDispositionItems",
                column: "GarmentInvoicePurchasingDisposistionId",
                principalTable: "GarmentInvoicePurchasingDispositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
