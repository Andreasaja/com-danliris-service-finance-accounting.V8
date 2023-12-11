using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class RemoveRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasings_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropIndex(
                name: "IX_MemoDetailGarmentPurchasingDetails_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.AddColumn<int>(
                name: "MemoDispositionId",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemoId",
                table: "MemoDetailGarmentPurchasingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MemoDetailGarmentPurchasingDispositions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    MemoDetailGarmentPurchasingId = table.Column<int>(nullable: false),
                    DispositionId = table.Column<int>(nullable: false),
                    DispositionNo = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoDetailGarmentPurchasingDispositions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemoDetailGarmentPurchasingDispositions");

            migrationBuilder.DropColumn(
                name: "MemoDispositionId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropColumn(
                name: "MemoId",
                table: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.CreateIndex(
                name: "IX_MemoDetailGarmentPurchasingDetails_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails",
                column: "MemoDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemoDetailGarmentPurchasingDetails_MemoDetailGarmentPurchasings_MemoDetailId",
                table: "MemoDetailGarmentPurchasingDetails",
                column: "MemoDetailId",
                principalTable: "MemoDetailGarmentPurchasings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
