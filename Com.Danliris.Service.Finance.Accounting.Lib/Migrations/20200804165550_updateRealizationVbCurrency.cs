using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class updateRealizationVbCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyDescription",
                table: "RealizationVbs",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbol",
                table: "RealizationVbs",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyDescription",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "CurrencySymbol",
                table: "RealizationVbs");
        }
    }
}
