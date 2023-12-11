using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class Add_MemoDetailGarmentPurchasing_MemoDetailGarmentPurchasingDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Memodate",
                table: "MemoGarmentPurchasings",
                newName: "MemoDate");

            migrationBuilder.CreateTable(
                name: "MemoDetailGarmentPurchasingDetails",
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
                    GarmentDeliveryOrderId = table.Column<int>(nullable: false),
                    GarmentDeliveryOrderNo = table.Column<string>(nullable: true),
                    RemarksDetail = table.Column<string>(nullable: true),
                    PaymentRate = table.Column<int>(nullable: false),
                    PurchasingRate = table.Column<int>(nullable: false),
                    MemoAmount = table.Column<int>(nullable: false),
                    MemoIdrAmount = table.Column<int>(nullable: false),
                    MemoDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoDetailGarmentPurchasingDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemoDetailGarmentPurchasings",
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
                    MemoId = table.Column<int>(nullable: false),
                    IsPosted = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoDetailGarmentPurchasings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemoDetailGarmentPurchasingDetails");

            migrationBuilder.DropTable(
                name: "MemoDetailGarmentPurchasings");

            migrationBuilder.RenameColumn(
                name: "MemoDate",
                table: "MemoGarmentPurchasings",
                newName: "Memodate");
        }
    }
}
