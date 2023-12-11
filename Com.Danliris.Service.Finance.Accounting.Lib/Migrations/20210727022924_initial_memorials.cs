using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class initial_memorials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentFinanceMemorials",
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
                    MemorialNo = table.Column<string>(maxLength: 20, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    AccountingBookId = table.Column<int>(nullable: false),
                    AccountingBookCode = table.Column<string>(maxLength: 10, nullable: true),
                    AccountingBookType = table.Column<string>(maxLength: 255, nullable: true),
                    GarmentCurrencyId = table.Column<int>(nullable: false),
                    GarmentCurrencyCode = table.Column<string>(maxLength: 20, nullable: true),
                    GarmentCurrencyRate = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceMemorials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentFinanceMemorialItems",
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
                    MemorialId = table.Column<int>(nullable: false),
                    COAId = table.Column<int>(nullable: false),
                    COACode = table.Column<string>(maxLength: 20, nullable: true),
                    COAName = table.Column<string>(maxLength: 100, nullable: true),
                    Debit = table.Column<double>(nullable: false),
                    Credit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceMemorialItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentFinanceMemorialItems_GarmentFinanceMemorials_MemorialId",
                        column: x => x.MemorialId,
                        principalTable: "GarmentFinanceMemorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinanceMemorialItems_MemorialId",
                table: "GarmentFinanceMemorialItems",
                column: "MemorialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentFinanceMemorialItems");

            migrationBuilder.DropTable(
                name: "GarmentFinanceMemorials");
        }
    }
}
