using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class VBRequestDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VBRequestDocumentEPODetails",
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
                    EPOId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 64, nullable: true),
                    ProductName = table.Column<string>(maxLength: 512, nullable: true),
                    DefaultQuantity = table.Column<double>(nullable: false),
                    DefaultUOMId = table.Column<int>(nullable: false),
                    DefaultUOMUnit = table.Column<string>(maxLength: 64, nullable: true),
                    DealQuantity = table.Column<double>(nullable: false),
                    DealUOMId = table.Column<int>(nullable: false),
                    DealUOMUnit = table.Column<string>(maxLength: 64, nullable: true),
                    Conversion = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UseVat = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VBRequestDocumentEPODetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VBRequestDocumentItems",
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
                    VBRequestDocumentId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 256, nullable: true),
                    UnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    DivisionId = table.Column<int>(nullable: false),
                    DivisionName = table.Column<string>(maxLength: 256, nullable: true),
                    DivisionCode = table.Column<string>(maxLength: 64, nullable: true),
                    EPOId = table.Column<int>(nullable: false),
                    EPONo = table.Column<string>(maxLength: 64, nullable: true),
                    UseIncomeTax = table.Column<bool>(nullable: false),
                    IncomeTaxId = table.Column<int>(nullable: false),
                    IncomeTaxName = table.Column<string>(maxLength: 64, nullable: true),
                    IncomeTaxRate = table.Column<double>(nullable: false),
                    IncomeTaxBy = table.Column<string>(maxLength: 64, nullable: true),
                    AmountByUnit = table.Column<double>(nullable: false),
                    IsSelected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VBRequestDocumentItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VBRequestDocuments",
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
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    RealizationEstimationDate = table.Column<DateTimeOffset>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencySymbol = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    CurrencyDescription = table.Column<string>(maxLength: 256, nullable: true),
                    Purpose = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    IsPosted = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SuppliantUnitId = table.Column<int>(nullable: false),
                    SuppliantUnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    SuppliantUnitName = table.Column<string>(maxLength: 256, nullable: true),
                    SuppliantDivisionId = table.Column<int>(nullable: false),
                    SuppliantDivisionCode = table.Column<string>(maxLength: 64, nullable: true),
                    SuppliantDivisionName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VBRequestDocuments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VBRequestDocumentEPODetails");

            migrationBuilder.DropTable(
                name: "VBRequestDocumentItems");

            migrationBuilder.DropTable(
                name: "VBRequestDocuments");
        }
    }
}
