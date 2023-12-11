using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateVBRealize2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyId",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrencyRate",
                table: "RealizationVbDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbol",
                table: "RealizationVbDetails",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                table: "RealizationVbDetails");

            migrationBuilder.DropColumn(
                name: "CurrencySymbol",
                table: "RealizationVbDetails");
        }
    }
}
