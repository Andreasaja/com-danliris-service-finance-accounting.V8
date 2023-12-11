using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Initial_PaymentDispositionNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentDispositionNotes",
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
                    BGCheckNumber = table.Column<string>(maxLength: 255, nullable: true),
                    BankAccountName = table.Column<string>(maxLength: 1000, nullable: true),
                    BankAccountNumber = table.Column<string>(maxLength: 255, nullable: true),
                    BankCode = table.Column<string>(maxLength: 255, nullable: true),
                    BankId = table.Column<int>(nullable: false),
                    BankName = table.Column<string>(maxLength: 1000, nullable: true),
                    BankCurrencyCode = table.Column<string>(maxLength: 255, nullable: true),
                    BankCurrencyId = table.Column<int>(nullable: false),
                    BankCurrencyRate = table.Column<double>(nullable: false),
                    PaymentDispositionNo = table.Column<string>(maxLength: 255, nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    SupplierCode = table.Column<string>(maxLength: 255, nullable: true),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierImport = table.Column<bool>(nullable: false),
                    SupplierName = table.Column<string>(maxLength: 1000, nullable: true),
                    PaymentDate = table.Column<DateTimeOffset>(nullable: false),
                    BankAccountCOA = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDispositionNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDispositionNoteItems",
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
                    PaymentDispositionNoteId = table.Column<int>(nullable: false),
                    DivisionId = table.Column<int>(nullable: false),
                    DivisionCode = table.Column<string>(maxLength: 255, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 500, nullable: true),
                    ProformaNo = table.Column<string>(maxLength: 255, nullable: true),
                    TotalPaid = table.Column<double>(nullable: false),
                    DispositionDate = table.Column<DateTimeOffset>(nullable: false),
                    DispositionId = table.Column<int>(nullable: false),
                    PurchasingDispositionExpeditionId = table.Column<int>(nullable: false),
                    DispositionNo = table.Column<string>(maxLength: 255, nullable: true),
                    DPP = table.Column<double>(nullable: false),
                    VatValue = table.Column<double>(nullable: false),
                    IncomeTaxValue = table.Column<double>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    CategoryCode = table.Column<string>(maxLength: 255, nullable: true),
                    CategoryName = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDispositionNoteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDispositionNoteItems_PaymentDispositionNotes_PaymentDispositionNoteId",
                        column: x => x.PaymentDispositionNoteId,
                        principalTable: "PaymentDispositionNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDispositionNoteDetails",
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
                    PaymentDispositionNoteItemId = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 255, nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 255, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 255, nullable: true),
                    PurchasingDispositionExpeditionItemId = table.Column<int>(nullable: false),
                    PurchasingDispositionDetailId = table.Column<int>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 255, nullable: true),
                    PaymentDispositionNoteItemModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDispositionNoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDispositionNoteDetails_PaymentDispositionNotes_PaymentDispositionNoteItemId",
                        column: x => x.PaymentDispositionNoteItemId,
                        principalTable: "PaymentDispositionNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentDispositionNoteDetails_PaymentDispositionNoteItems_PaymentDispositionNoteItemModelId",
                        column: x => x.PaymentDispositionNoteItemModelId,
                        principalTable: "PaymentDispositionNoteItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDispositionNoteDetails_PaymentDispositionNoteItemId",
                table: "PaymentDispositionNoteDetails",
                column: "PaymentDispositionNoteItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDispositionNoteDetails_PaymentDispositionNoteItemModelId",
                table: "PaymentDispositionNoteDetails",
                column: "PaymentDispositionNoteItemModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDispositionNoteItems_PaymentDispositionNoteId",
                table: "PaymentDispositionNoteItems",
                column: "PaymentDispositionNoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDispositionNoteDetails");

            migrationBuilder.DropTable(
                name: "PaymentDispositionNoteItems");

            migrationBuilder.DropTable(
                name: "PaymentDispositionNotes");
        }
    }
}
