using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_Table_GarmentFinanceBankCashReceiptDetailOtherItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentFinanceBankCashReceiptDetailOtherItems",
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
                    BankCashReceiptDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentFinanceBankCashReceiptDetailOtherItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentFinanceBankCashReceiptDetailOtherItems_GarmentFinanceBankCashReceiptDetails_BankCashReceiptDetailId",
                        column: x => x.BankCashReceiptDetailId,
                        principalTable: "GarmentFinanceBankCashReceiptDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentFinanceBankCashReceiptDetailOtherItems_BankCashReceiptDetailId",
                table: "GarmentFinanceBankCashReceiptDetailOtherItems",
                column: "BankCashReceiptDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentFinanceBankCashReceiptDetailOtherItems");
        }
    }
}
