using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateVbRequestAndVbRequestDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUseVat",
                table: "VbRequestsDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxBy",
                table: "VbRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxId",
                table: "VbRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxName",
                table: "VbRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxRate",
                table: "VbRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUseVat",
                table: "VbRequestsDetails");

            migrationBuilder.DropColumn(
                name: "IncomeTaxBy",
                table: "VbRequests");

            migrationBuilder.DropColumn(
                name: "IncomeTaxId",
                table: "VbRequests");

            migrationBuilder.DropColumn(
                name: "IncomeTaxName",
                table: "VbRequests");

            migrationBuilder.DropColumn(
                name: "IncomeTaxRate",
                table: "VbRequests");
        }
    }
}
