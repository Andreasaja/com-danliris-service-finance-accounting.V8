using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_Table_BankCashReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankCashReceiptModel",
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
                    ReceiptNo = table.Column<string>(maxLength: 256, nullable: true),
                    ReceiptDate = table.Column<DateTimeOffset>(nullable: false),
                    BankAccountId = table.Column<int>(nullable: false),
                    BankAccountNumber = table.Column<string>(maxLength: 64, nullable: true),
                    BankName = table.Column<string>(maxLength: 256, nullable: true),
                    BankAccountingCode = table.Column<string>(maxLength: 32, nullable: true),
                    BankCurrencyCode = table.Column<string>(maxLength: 32, nullable: true),
                    BankCurrencyId = table.Column<int>(nullable: false),
                    BankCurrencyRate = table.Column<double>(nullable: false),
                    DebitCoaId = table.Column<int>(nullable: false),
                    DebitCoaCode = table.Column<string>(maxLength: 32, nullable: true),
                    DebitCoaName = table.Column<string>(maxLength: 256, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    NumberingCode = table.Column<string>(maxLength: 32, nullable: true),
                    IncomeType = table.Column<string>(maxLength: 256, nullable: true),
                    Remarks = table.Column<string>(maxLength: 1024, nullable: true),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankCashReceiptModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankCashReceiptItemModel",
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
                    AccNumberCoaId = table.Column<int>(nullable: false),
                    AccNumberCoaCode = table.Column<string>(maxLength: 32, nullable: true),
                    AccNumberCoaName = table.Column<string>(maxLength: 256, nullable: true),
                    AccSubCoaId = table.Column<int>(nullable: false),
                    AccSubCode = table.Column<string>(maxLength: 32, nullable: true),
                    AccSubCoaName = table.Column<string>(maxLength: 256, nullable: true),
                    AccUnitCoaId = table.Column<int>(nullable: false),
                    AccUnitCode = table.Column<string>(maxLength: 32, nullable: true),
                    AccUnitCoaName = table.Column<string>(maxLength: 256, nullable: true),
                    AccAmountCoaId = table.Column<int>(nullable: false),
                    AccAmountCode = table.Column<string>(maxLength: 32, nullable: true),
                    AccAmountCoaName = table.Column<string>(maxLength: 256, nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Summary = table.Column<decimal>(nullable: false),
                    C2A = table.Column<decimal>(nullable: false),
                    C2B = table.Column<decimal>(nullable: false),
                    C2C = table.Column<decimal>(nullable: false),
                    C1A = table.Column<decimal>(nullable: false),
                    C1B = table.Column<decimal>(nullable: false),
                    NoteNumber = table.Column<string>(maxLength: 256, nullable: true),
                    Remarks = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankCashReceiptItemModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankCashReceiptItemModel_BankCashReceiptModel_BankCashReceiptId",
                        column: x => x.BankCashReceiptId,
                        principalTable: "BankCashReceiptModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankCashReceiptItemModel_BankCashReceiptId",
                table: "BankCashReceiptItemModel",
                column: "BankCashReceiptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankCashReceiptItemModel");

            migrationBuilder.DropTable(
                name: "BankCashReceiptModel");
        }
    }
}
