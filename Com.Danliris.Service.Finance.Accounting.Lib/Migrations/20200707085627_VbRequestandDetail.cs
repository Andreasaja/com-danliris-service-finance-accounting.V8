using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class VbRequestandDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VbRequests",
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
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    DateEstimate = table.Column<DateTimeOffset>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 64, nullable: true),
                    UnitName = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 64, nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    CurrencySymbol = table.Column<string>(maxLength: 64, nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Usage = table.Column<string>(maxLength: 255, nullable: true),
                    UnitLoad = table.Column<string>(maxLength: 255, nullable: true),
                    Apporve_Status = table.Column<bool>(nullable: false),
                    Complete_Status = table.Column<bool>(nullable: false),
                    VBRequestCategory = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VbRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VbRequestsDetails",
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
                    POId = table.Column<int>(nullable: false),
                    PONo = table.Column<string>(maxLength: 64, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 64, nullable: true),
                    DetailOthers = table.Column<string>(maxLength: 64, nullable: true),
                    ProductId = table.Column<string>(maxLength: 64, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 64, nullable: true),
                    ProductName = table.Column<string>(maxLength: 64, nullable: true),
                    DefaultQuantity = table.Column<decimal>(nullable: false),
                    DefaultUOMId = table.Column<string>(maxLength: 64, nullable: true),
                    DefaultUOMUnit = table.Column<string>(maxLength: 64, nullable: true),
                    DealQuantity = table.Column<decimal>(nullable: false),
                    DealUOMId = table.Column<string>(maxLength: 64, nullable: true),
                    DealUOMUnit = table.Column<string>(maxLength: 64, nullable: true),
                    Conversion = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductRemark = table.Column<string>(maxLength: 64, nullable: true),
                    VBId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VbRequestsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VbRequestsDetails_VbRequests_VBId",
                        column: x => x.VBId,
                        principalTable: "VbRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VbRequestsDetails_VBId",
                table: "VbRequestsDetails",
                column: "VBId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VbRequestsDetails");

            migrationBuilder.DropTable(
                name: "VbRequests");
        }
    }
}
