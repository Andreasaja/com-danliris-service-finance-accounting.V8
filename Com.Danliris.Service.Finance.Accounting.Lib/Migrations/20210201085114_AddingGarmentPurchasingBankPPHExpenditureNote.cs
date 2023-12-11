using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddingGarmentPurchasingBankPPHExpenditureNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentPurchasingPphBankExpenditureNotes",
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
                    InvoiceOutNumber = table.Column<string>(nullable: true),
                    InvoiceOutDate = table.Column<DateTimeOffset>(nullable: false),
                    DueDateStart = table.Column<DateTimeOffset>(nullable: false),
                    DueDateEnd = table.Column<DateTimeOffset>(nullable: false),
                    IncomeTaxId = table.Column<int>(nullable: false),
                    IncomeTaxName = table.Column<string>(nullable: true),
                    IncomeTaxRate = table.Column<double>(nullable: false),
                    AccountBankCOA = table.Column<string>(nullable: true),
                    AccountBankName = table.Column<string>(nullable: true),
                    AccountBankNumber = table.Column<string>(nullable: true),
                    BankAddress = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankCode1 = table.Column<string>(nullable: true),
                    BankCurrencyCode = table.Column<string>(nullable: true),
                    BankCurrencyId = table.Column<int>(nullable: false),
                    BankSwiftCode = table.Column<string>(nullable: true),
                    IsPosted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentPurchasingPphBankExpenditureNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentPurchasingPphBankExpenditureNoteItems",
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
                    InternalNotesNo = table.Column<string>(nullable: true),
                    InternalNotesId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    DueDate = table.Column<DateTimeOffset>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierName = table.Column<string>(nullable: true),
                    VAT = table.Column<double>(nullable: false),
                    CorrectionAmount = table.Column<double>(nullable: false),
                    IncomeTaxTotal = table.Column<double>(nullable: false),
                    IncomeTaxRate = table.Column<double>(nullable: false),
                    IncomeTaxId = table.Column<int>(nullable: false),
                    IncomeTaxName = table.Column<string>(nullable: true),
                    TotalPaid = table.Column<double>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true),
                    AmountDPP = table.Column<double>(nullable: false),
                    PaymentType = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    PaymentDueDays = table.Column<int>(nullable: false),
                    GarmentPurchasingPphBankExpenditureNoteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentPurchasingPphBankExpenditureNoteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentPurchasingPphBankExpenditureNoteItems_GarmentPurchasingPphBankExpenditureNotes_GarmentPurchasingPphBankExpenditureNot~",
                        column: x => x.GarmentPurchasingPphBankExpenditureNoteId,
                        principalTable: "GarmentPurchasingPphBankExpenditureNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentPurchasingPphBankExpenditureNoteInvoices",
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
                    InvoicesNo = table.Column<string>(nullable: true),
                    InvoicesDate = table.Column<DateTimeOffset>(nullable: false),
                    InvoicesId = table.Column<long>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    ProductCategory = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    GarmentPurchasingPphBankExpenditureNoteItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentPurchasingPphBankExpenditureNoteInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentPurchasingPphBankExpenditureNoteInvoices_GarmentPurchasingPphBankExpenditureNoteItems_GarmentPurchasingPphBankExpendi~",
                        column: x => x.GarmentPurchasingPphBankExpenditureNoteItemId,
                        principalTable: "GarmentPurchasingPphBankExpenditureNoteItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentPurchasingPphBankExpenditureNoteInvoices_GarmentPurchasingPphBankExpenditureNoteItemId",
                table: "GarmentPurchasingPphBankExpenditureNoteInvoices",
                column: "GarmentPurchasingPphBankExpenditureNoteItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentPurchasingPphBankExpenditureNoteItems_GarmentPurchasingPphBankExpenditureNoteId",
                table: "GarmentPurchasingPphBankExpenditureNoteItems",
                column: "GarmentPurchasingPphBankExpenditureNoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentPurchasingPphBankExpenditureNoteInvoices");

            migrationBuilder.DropTable(
                name: "GarmentPurchasingPphBankExpenditureNoteItems");

            migrationBuilder.DropTable(
                name: "GarmentPurchasingPphBankExpenditureNotes");
        }
    }
}
