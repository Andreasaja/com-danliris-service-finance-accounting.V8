using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_Table_PurchasingDispositionExpedition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasingDispositionExpeditions",
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
                    BankExpenditureNoteNo = table.Column<string>(nullable: true),
                    CashierDivisionBy = table.Column<string>(nullable: true),
                    CashierDivisionDate = table.Column<DateTimeOffset>(nullable: true),
                    CurrencyId = table.Column<string>(maxLength: 50, nullable: true),
                    CurrencyCode = table.Column<string>(maxLength: 255, nullable: true),
                    PaymentDueDate = table.Column<DateTimeOffset>(nullable: false),
                    InvoiceNo = table.Column<string>(nullable: true),
                    NotVerifiedReason = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    SendToCashierDivisionBy = table.Column<string>(nullable: true),
                    SendToCashierDivisionDate = table.Column<DateTimeOffset>(nullable: true),
                    SendToPurchasingDivisionBy = table.Column<string>(nullable: true),
                    SendToPurchasingDivisionDate = table.Column<DateTimeOffset>(nullable: true),
                    SupplierCode = table.Column<string>(maxLength: 255, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 255, nullable: true),
                    TotalPaid = table.Column<double>(nullable: false),
                    DispositionId = table.Column<string>(nullable: true),
                    DispositionDate = table.Column<DateTimeOffset>(nullable: false),
                    DispositionNo = table.Column<string>(nullable: true),
                    VerificationDivisionBy = table.Column<string>(nullable: true),
                    VerificationDivisionDate = table.Column<DateTimeOffset>(nullable: true),
                    VerifyDate = table.Column<DateTimeOffset>(nullable: true),
                    UseIncomeTax = table.Column<bool>(nullable: false),
                    IncomeTax = table.Column<double>(nullable: false),
                    IncomeTaxId = table.Column<string>(nullable: true),
                    IncomeTaxName = table.Column<string>(nullable: true),
                    IncomeTaxRate = table.Column<double>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    IsPaidPPH = table.Column<bool>(nullable: false),
                    UseVat = table.Column<bool>(nullable: false),
                    Vat = table.Column<double>(nullable: false),
                    BankExpenditureNoteDate = table.Column<DateTimeOffset>(nullable: true),
                    BankExpenditureNotePPHDate = table.Column<DateTimeOffset>(nullable: true),
                    BankExpenditureNotePPHNo = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    CategoryCode = table.Column<string>(maxLength: 255, nullable: true),
                    CategoryName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingDispositionExpeditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasingDispositionExpeditionItems",
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
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<string>(maxLength: 50, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 255, nullable: true),
                    ProductName = table.Column<string>(maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UnitId = table.Column<string>(maxLength: 50, nullable: true),
                    UnitCode = table.Column<string>(maxLength: 255, nullable: true),
                    UnitName = table.Column<string>(maxLength: 255, nullable: true),
                    Uom = table.Column<string>(nullable: true),
                    PurchasingDispositionDetailId = table.Column<int>(nullable: false),
                    PurchasingDispositionExpeditionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasingDispositionExpeditionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasingDispositionExpeditionItems_PurchasingDispositionExpeditions_PurchasingDispositionExpeditionId",
                        column: x => x.PurchasingDispositionExpeditionId,
                        principalTable: "PurchasingDispositionExpeditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasingDispositionExpeditionItems_PurchasingDispositionExpeditionId",
                table: "PurchasingDispositionExpeditionItems",
                column: "PurchasingDispositionExpeditionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasingDispositionExpeditionItems");

            migrationBuilder.DropTable(
                name: "PurchasingDispositionExpeditions");
        }
    }
}
