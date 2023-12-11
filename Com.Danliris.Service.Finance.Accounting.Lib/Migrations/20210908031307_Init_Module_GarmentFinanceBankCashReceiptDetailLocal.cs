using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Init_Module_GarmentFinanceBankCashReceiptDetailLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentFinanceBankCashReceiptDetailLocals",
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
                    BankCashReceiptId = table.Column<int>(nullable: false),
                    BankCashReceiptNo = table.Column<string>(maxLength: 256, nullable: true),
                    BankCashReceiptDate = table.Column<DateTimeOffset>(nullable: false),
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
                    table.PrimaryKey("PK_GarmentFinanceBankCashReceiptDetailLocals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentFinanceBankCashReceiptDetailLocalItems",
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
                    LocalSalesNoteId = table.Column<int>(nullable: false),
                    LocalSalesNoteNo = table.Column<string>(nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 255, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 1000, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 32, nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    BankCashReceiptDetailLocalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceBankCashReceiptDetailLocalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentFinanceBankCashReceiptDetailLocalItems_GarmentFinanceBankCashReceiptDetailLocals_BankCashReceiptDetailLocalId",
                        column: x => x.BankCashReceiptDetailLocalId,
                        principalTable: "GarmentFinanceBankCashReceiptDetailLocals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentFinanceBankCashReceiptDetailLocalOtherItems",
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
                    Remarks = table.Column<string>(maxLength: 1000, nullable: true),
                    TypeAmount = table.Column<string>(maxLength: 32, nullable: true),
                    BankCashReceiptDetailLocalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceBankCashReceiptDetailLocalOtherItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentFinanceBankCashReceiptDetailLocalOtherItems_GarmentFinanceBankCashReceiptDetailLocals_BankCashReceiptDetailLocalId",
                        column: x => x.BankCashReceiptDetailLocalId,
                        principalTable: "GarmentFinanceBankCashReceiptDetailLocals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinanceBankCashReceiptDetailLocalItems_BankCashReceiptDetailLocalId",
                table: "GarmentFinanceBankCashReceiptDetailLocalItems",
                column: "BankCashReceiptDetailLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinanceBankCashReceiptDetailLocalOtherItems_BankCashReceiptDetailLocalId",
                table: "GarmentFinanceBankCashReceiptDetailLocalOtherItems",
                column: "BankCashReceiptDetailLocalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentFinanceBankCashReceiptDetailLocalItems");

            migrationBuilder.DropTable(
                name: "GarmentFinanceBankCashReceiptDetailLocalOtherItems");

            migrationBuilder.DropTable(
                name: "GarmentFinanceBankCashReceiptDetailLocals");
        }
    }
}
