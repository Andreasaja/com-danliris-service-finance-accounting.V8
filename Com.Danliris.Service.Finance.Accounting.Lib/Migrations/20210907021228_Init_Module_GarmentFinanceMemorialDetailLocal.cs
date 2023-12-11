using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Init_Module_GarmentFinanceMemorialDetailLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentFinanceMemorialDetailLocals",
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
                    MemorialId = table.Column<int>(nullable: false),
                    MemorialDate = table.Column<DateTimeOffset>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    DebitCoaId = table.Column<int>(nullable: false),
                    DebitCoaCode = table.Column<string>(maxLength: 32, nullable: true),
                    DebitCoaName = table.Column<string>(maxLength: 256, nullable: true),
                    InvoiceCoaId = table.Column<int>(nullable: false),
                    InvoiceCoaCode = table.Column<string>(maxLength: 32, nullable: true),
                    InvoiceCoaName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceMemorialDetailLocals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentFinanceMemorialDetailLocalItems",
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
                    MemorialDetailLocalId = table.Column<int>(nullable: false),
                    LocalSalesNoteId = table.Column<int>(nullable: false),
                    LocalSalesNoteNo = table.Column<string>(maxLength: 20, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerName = table.Column<string>(maxLength: 225, nullable: true),
                    BuyerCode = table.Column<string>(maxLength: 20, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 20, nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceMemorialDetailLocalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentFinanceMemorialDetailLocalItems_GarmentFinanceMemorialDetailLocals_MemorialDetailLocalId",
                        column: x => x.MemorialDetailLocalId,
                        principalTable: "GarmentFinanceMemorialDetailLocals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentFinanceMemorialDetailLocalOtherItems",
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
                    ChartOfAccountId = table.Column<int>(nullable: false),
                    ChartOfAccountCode = table.Column<string>(maxLength: 32, nullable: true),
                    ChartOfAccountName = table.Column<string>(maxLength: 255, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 32, nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    TypeAmount = table.Column<string>(maxLength: 32, nullable: true),
                    Remarks = table.Column<string>(maxLength: 1000, nullable: true),
                    MemorialDetailLocalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceMemorialDetailLocalOtherItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentFinanceMemorialDetailLocalOtherItems_GarmentFinanceMemorialDetailLocals_MemorialDetailLocalId",
                        column: x => x.MemorialDetailLocalId,
                        principalTable: "GarmentFinanceMemorialDetailLocals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinanceMemorialDetailLocalItems_MemorialDetailLocalId",
                table: "GarmentFinanceMemorialDetailLocalItems",
                column: "MemorialDetailLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinanceMemorialDetailLocalOtherItems_MemorialDetailLocalId",
                table: "GarmentFinanceMemorialDetailLocalOtherItems",
                column: "MemorialDetailLocalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentFinanceMemorialDetailLocalItems");

            migrationBuilder.DropTable(
                name: "GarmentFinanceMemorialDetailLocalOtherItems");

            migrationBuilder.DropTable(
                name: "GarmentFinanceMemorialDetailLocals");
        }
    }
}
