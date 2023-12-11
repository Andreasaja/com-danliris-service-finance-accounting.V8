using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateVBRealize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Apporve_Status",
                table: "VbRequests",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RealizationVbs",
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
                    VBNo = table.Column<string>(maxLength: 64, nullable: true),
                    VBNoRealize = table.Column<string>(maxLength: 64, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    DateEstimate = table.Column<DateTimeOffset>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    UnitName = table.Column<string>(maxLength: 64, nullable: true),
                    RequestVbName = table.Column<string>(maxLength: 64, nullable: true),
                    isVerified = table.Column<bool>(nullable: false),
                    VerifiedDate = table.Column<DateTimeOffset>(nullable: false),
                    isClosed = table.Column<bool>(nullable: false),
                    CloseDate = table.Column<DateTimeOffset>(nullable: false),
                    isNotVeridied = table.Column<bool>(nullable: false),
                    VBRealizeCategory = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealizationVbs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealizationVbDetails",
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
                    DivisionSPB = table.Column<string>(maxLength: 64, nullable: true),
                    NoSPB = table.Column<string>(maxLength: 64, nullable: true),
                    DateSPB = table.Column<DateTimeOffset>(nullable: true),
                    SupplierCode = table.Column<string>(maxLength: 64, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 64, nullable: true),
                    NoPOSPB = table.Column<string>(maxLength: 64, nullable: true),
                    PriceTotalSPB = table.Column<decimal>(nullable: false),
                    IdProductSPB = table.Column<string>(maxLength: 64, nullable: true),
                    CodeProductSPB = table.Column<string>(maxLength: 64, nullable: true),
                    NameProductSPB = table.Column<string>(maxLength: 64, nullable: true),
                    VBRealizationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealizationVbDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealizationVbDetails_RealizationVbs_VBRealizationId",
                        column: x => x.VBRealizationId,
                        principalTable: "RealizationVbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealizationVbDetails_VBRealizationId",
                table: "RealizationVbDetails",
                column: "VBRealizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealizationVbDetails");

            migrationBuilder.DropTable(
                name: "RealizationVbs");

            migrationBuilder.AlterColumn<bool>(
                name: "Apporve_Status",
                table: "VbRequests",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
