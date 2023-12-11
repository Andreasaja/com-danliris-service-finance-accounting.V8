using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class NewFixMigrationFinance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Memos",
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
                    DocumentNo = table.Column<string>(maxLength: 64, nullable: true),
                    SalesInvoiceId = table.Column<int>(nullable: false),
                    SalesInvoiceNo = table.Column<string>(maxLength: 64, nullable: true),
                    MemoType = table.Column<string>(maxLength: 64, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerName = table.Column<string>(maxLength: 512, nullable: true),
                    BuyerCode = table.Column<string>(maxLength: 64, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 64, nullable: true),
                    UnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesReceipts",
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
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    AutoIncreament = table.Column<long>(nullable: false),
                    SalesReceiptNo = table.Column<string>(maxLength: 255, nullable: true),
                    SalesReceiptDate = table.Column<DateTimeOffset>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 255, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerName = table.Column<string>(maxLength: 255, nullable: true),
                    BuyerAddress = table.Column<string>(maxLength: 1000, nullable: true),
                    OriginAccountNumber = table.Column<string>(maxLength: 255, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 255, nullable: true),
                    CurrencySymbol = table.Column<string>(maxLength: 255, nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    AccountName = table.Column<string>(maxLength: 255, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 255, nullable: true),
                    BankName = table.Column<string>(maxLength: 255, nullable: true),
                    BankCode = table.Column<string>(maxLength: 255, nullable: true),
                    AdministrationFee = table.Column<double>(nullable: false),
                    TotalPaid = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReceipts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemoItems",
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
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    PaymentAmount = table.Column<decimal>(nullable: false),
                    Interest = table.Column<decimal>(nullable: false),
                    MemoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemoItems_Memos_MemoId",
                        column: x => x.MemoId,
                        principalTable: "Memos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesReceiptDetails",
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
                    SalesInvoiceId = table.Column<int>(nullable: false),
                    SalesInvoiceNo = table.Column<string>(maxLength: 255, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 255, nullable: true),
                    CurrencySymbol = table.Column<string>(maxLength: 255, nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    DueDate = table.Column<DateTimeOffset>(nullable: false),
                    VatType = table.Column<string>(maxLength: 255, nullable: true),
                    Tempo = table.Column<double>(nullable: false),
                    TotalPayment = table.Column<double>(nullable: false),
                    TotalPaid = table.Column<double>(nullable: false),
                    Paid = table.Column<double>(nullable: false),
                    Nominal = table.Column<double>(nullable: false),
                    Unpaid = table.Column<double>(nullable: false),
                    OverPaid = table.Column<double>(nullable: false),
                    IsPaidOff = table.Column<bool>(nullable: false),
                    SalesReceiptId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReceiptDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesReceiptDetails_SalesReceipts_SalesReceiptId",
                        column: x => x.SalesReceiptId,
                        principalTable: "SalesReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemoItems_MemoId",
                table: "MemoItems",
                column: "MemoId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReceiptDetails_SalesReceiptId",
                table: "SalesReceiptDetails",
                column: "SalesReceiptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemoItems");

            migrationBuilder.DropTable(
                name: "SalesReceiptDetails");

            migrationBuilder.DropTable(
                name: "Memos");

            migrationBuilder.DropTable(
                name: "SalesReceipts");
        }
    }
}
