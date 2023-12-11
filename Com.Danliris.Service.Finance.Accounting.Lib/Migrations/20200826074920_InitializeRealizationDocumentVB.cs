using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class InitializeRealizationDocumentVB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VBRealizationDocumentExpenditureItems",
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
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    UseVat = table.Column<bool>(nullable: false),
                    UseIncomeTax = table.Column<bool>(nullable: false),
                    IncomeTaxId = table.Column<int>(nullable: false),
                    IncomeTaxName = table.Column<string>(maxLength: 64, nullable: true),
                    IncomeTaxRate = table.Column<double>(nullable: false),
                    IncomeTaxBy = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VBRealizationDocumentExpenditureItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VBRealizationDocuments",
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
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    VBRequestDocumentId = table.Column<int>(nullable: false),
                    VBRequestDocumentNo = table.Column<string>(maxLength: 64, nullable: true),
                    VBRequestDocumentDate = table.Column<DateTimeOffset>(nullable: true),
                    VBRequestDocumentRealizationEstimationDate = table.Column<DateTimeOffset>(nullable: true),
                    SuppliantUnitId = table.Column<int>(nullable: false),
                    SuppliantUnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    SuppliantUnitName = table.Column<string>(maxLength: 64, nullable: true),
                    SuppliantDivisionId = table.Column<int>(nullable: false),
                    SuppliantDivisionCode = table.Column<string>(maxLength: 64, nullable: true),
                    SuppliantDivisionName = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencySymbol = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyRate = table.Column<double>(nullable: false),
                    CurrencyDescription = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VBRealizationDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VBRealizationDocumentUnitCostsItems",
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
                    UnitId = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 256, nullable: true),
                    UnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    DivisionId = table.Column<int>(nullable: false),
                    DivisionName = table.Column<string>(maxLength: 256, nullable: true),
                    DivisionCode = table.Column<string>(maxLength: 64, nullable: true),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VBRealizationDocumentUnitCostsItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VBRealizationDocumentExpenditureItems");

            migrationBuilder.DropTable(
                name: "VBRealizationDocuments");

            migrationBuilder.DropTable(
                name: "VBRealizationDocumentUnitCostsItems");
        }
    }
}
