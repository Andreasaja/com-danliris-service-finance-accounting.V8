using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddForeignKey_GarmentFinanceMemorialDetailRupiahItemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemorialDetailId",
                table: "GarmentFinanceMemorialDetailRupiahItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinanceMemorialDetailRupiahItems_MemorialDetailId",
                table: "GarmentFinanceMemorialDetailRupiahItems",
                column: "MemorialDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentFinanceMemorialDetailRupiahItems_GarmentFinanceMemorialDetails_MemorialDetailId",
                table: "GarmentFinanceMemorialDetailRupiahItems",
                column: "MemorialDetailId",
                principalTable: "GarmentFinanceMemorialDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentFinanceMemorialDetailRupiahItems_GarmentFinanceMemorialDetails_MemorialDetailId",
                table: "GarmentFinanceMemorialDetailRupiahItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentFinanceMemorialDetailRupiahItems_MemorialDetailId",
                table: "GarmentFinanceMemorialDetailRupiahItems");

            migrationBuilder.DropColumn(
                name: "MemorialDetailId",
                table: "GarmentFinanceMemorialDetailRupiahItems");
        }
    }
}
