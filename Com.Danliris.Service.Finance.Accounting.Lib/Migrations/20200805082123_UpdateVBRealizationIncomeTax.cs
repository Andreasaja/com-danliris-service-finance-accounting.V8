using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateVBRealizationIncomeTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "IncomeTaxAmount",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxBy",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxId",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxName",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxRate",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isGetPPh",
                table: "RealizationVbDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeTaxAmount",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "IncomeTaxBy",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "IncomeTaxId",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "IncomeTaxName",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "IncomeTaxRate",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "isGetPPh",
                table: "RealizationVbDetails");
        }
    }
}
