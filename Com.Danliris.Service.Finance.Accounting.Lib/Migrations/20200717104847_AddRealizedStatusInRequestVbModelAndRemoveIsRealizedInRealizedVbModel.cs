using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class AddRealizedStatusInRequestVbModelAndRemoveIsRealizedInRealizedVbModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isRealized",
                table: "RealizationVbs");

            migrationBuilder.AddColumn<bool>(
                name: "Realization_Status",
                table: "VbRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Realization_Status",
                table: "VbRequests");

            migrationBuilder.AddColumn<bool>(
                name: "isRealized",
                table: "RealizationVbs",
                nullable: false,
                defaultValue: false);
        }
    }
}
