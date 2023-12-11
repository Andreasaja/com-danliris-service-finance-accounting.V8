using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddIsRealizedInRealizedVbModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isRealized",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ClearanceVBModel",
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
                    RqstNo = table.Column<string>(maxLength: 64, nullable: true),
                    VBCategory = table.Column<string>(maxLength: 255, nullable: true),
                    RqstDate = table.Column<DateTimeOffset>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(maxLength: 64, nullable: true),
                    Appliciant = table.Column<string>(maxLength: 255, nullable: true),
                    ClearanceDate = table.Column<DateTimeOffset>(nullable: true),
                    IsPosted = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    RealNo = table.Column<string>(maxLength: 64, nullable: true),
                    RealDate = table.Column<DateTimeOffset>(nullable: false),
                    VerDate = table.Column<DateTimeOffset>(nullable: true),
                    DiffStatus = table.Column<string>(nullable: true),
                    DiffAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearanceVBModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClearanceVBModel");

            migrationBuilder.DropColumn(
                name: "isRealized",
                table: "RealizationVbs");
        }
    }
}
