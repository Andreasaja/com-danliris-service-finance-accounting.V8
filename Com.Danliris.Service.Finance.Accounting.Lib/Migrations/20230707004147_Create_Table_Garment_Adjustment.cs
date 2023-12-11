using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Create_Table_Garment_Adjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentFinanceAdjustments",
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
                    AdjustmentNo = table.Column<string>(maxLength: 20, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    GarmentCurrencyId = table.Column<int>(nullable: false),
                    GarmentCurrencyCode = table.Column<string>(maxLength: 20, nullable: true),
                    GarmentCurrencyRate = table.Column<double>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(maxLength: 4000, nullable: true),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceAdjustments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentFinanceAdjustmentItems",
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
                    AdjustmentId = table.Column<int>(nullable: false),
                    COAId = table.Column<int>(nullable: false),
                    COACode = table.Column<string>(maxLength: 20, nullable: true),
                    COAName = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Debit = table.Column<double>(nullable: false),
                    Credit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceAdjustmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentFinanceAdjustmentItems_GarmentFinanceAdjustments_AdjustmentId",
                        column: x => x.AdjustmentId,
                        principalTable: "GarmentFinanceAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinanceAdjustmentItems_AdjustmentId",
                table: "GarmentFinanceAdjustmentItems",
                column: "AdjustmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentFinanceAdjustmentItems");

            migrationBuilder.DropTable(
                name: "GarmentFinanceAdjustments");
        }
    }
}
