using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Initial_GarmentInvoicePayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentInvoicePayments",
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
                    InvoicePaymentNo = table.Column<string>(maxLength: 50, nullable: true),
                    PaymentDate = table.Column<DateTimeOffset>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 255, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerName = table.Column<string>(maxLength: 1000, nullable: true),
                    BGNo = table.Column<string>(maxLength: 100, nullable: true),
                    CurrencyCode = table.Column<string>(maxLength: 255, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyRate = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentInvoicePayments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentInvoicePaymentItems",
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
                    InvoicePaymentId = table.Column<int>(nullable: false),
                    InvoiceId = table.Column<int>(nullable: false),
                    InvoiceNo = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    IDRAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentInvoicePaymentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentInvoicePaymentItems_GarmentInvoicePayments_InvoicePaymentId",
                        column: x => x.InvoicePaymentId,
                        principalTable: "GarmentInvoicePayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentInvoicePaymentItems_InvoicePaymentId",
                table: "GarmentInvoicePaymentItems",
                column: "InvoicePaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentInvoicePaymentItems");

            migrationBuilder.DropTable(
                name: "GarmentInvoicePayments");
        }
    }
}
