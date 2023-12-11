using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class addingGarmentInvoicePurchasingDisposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentInvoicePurchasingDispositions",
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
                    InvoiceNo = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTimeOffset>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true),
                    BankAccount = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    CurrencyDate = table.Column<DateTimeOffset>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    SupplierName = table.Column<string>(nullable: true),
                    SupplierCode = table.Column<string>(nullable: true),
                    IsImportSupplier = table.Column<bool>(nullable: false),
                    ChequeNo = table.Column<string>(nullable: true),
                    PaymentType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentInvoicePurchasingDispositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentInvoicePurchasingDispositionItems",
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
                    DispositionId = table.Column<int>(nullable: false),
                    DispositionNo = table.Column<string>(nullable: true),
                    DispositionDate = table.Column<DateTimeOffset>(nullable: false),
                    DipositionDueDate = table.Column<DateTimeOffset>(nullable: false),
                    ProformaNo = table.Column<string>(nullable: true),
                    SupplierName = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    VATAmount = table.Column<double>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    TotalPaid = table.Column<double>(nullable: false),
                    TotalPaidBefore = table.Column<double>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    GarmentInvoicePurchasingDisposistionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentInvoicePurchasingDispositionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDispositions_GarmentInvoicePurchasingDisposistionId",
                        column: x => x.GarmentInvoicePurchasingDisposistionId,
                        principalTable: "GarmentInvoicePurchasingDispositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentInvoicePurchasingDispositionItems_GarmentInvoicePurchasingDisposistionId",
                table: "GarmentInvoicePurchasingDispositionItems",
                column: "GarmentInvoicePurchasingDisposistionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentInvoicePurchasingDispositionItems");

            migrationBuilder.DropTable(
                name: "GarmentInvoicePurchasingDispositions");
        }
    }
}
