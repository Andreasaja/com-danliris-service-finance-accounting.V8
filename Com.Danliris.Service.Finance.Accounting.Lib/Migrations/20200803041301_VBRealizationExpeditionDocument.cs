using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class VBRealizationExpeditionDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VBRealizationDocumentExpeditions",
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
                    VBRealizationId = table.Column<int>(nullable: false),
                    VBId = table.Column<int>(nullable: false),
                    VBNo = table.Column<string>(maxLength: 64, nullable: true),
                    VBRealizationNo = table.Column<string>(maxLength: 64, nullable: true),
                    VBRealizationDate = table.Column<DateTimeOffset>(nullable: false),
                    VBRequestName = table.Column<string>(maxLength: 256, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 64, nullable: true),
                    DivisionId = table.Column<int>(nullable: false),
                    DivisionName = table.Column<string>(maxLength: 64, nullable: true),
                    VBAmount = table.Column<decimal>(nullable: false),
                    VBRealizationAmount = table.Column<decimal>(nullable: false),
                    SendToVerificationBy = table.Column<string>(maxLength: 256, nullable: true),
                    SendToVerificationDate = table.Column<DateTimeOffset>(nullable: true),
                    VerifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    VerifedDate = table.Column<DateTimeOffset>(nullable: true),
                    SendToCashierBy = table.Column<string>(maxLength: 256, nullable: true),
                    SendToCashierDate = table.Column<DateTimeOffset>(nullable: true),
                    CashierBy = table.Column<string>(maxLength: 256, nullable: true),
                    CashierDate = table.Column<DateTimeOffset>(nullable: true),
                    NotVerifiedReason = table.Column<string>(nullable: true),
                    NotVerifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    NotVerifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VBRealizationDocumentExpeditions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VBRealizationDocumentExpeditions");
        }
    }
}
