using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateTabelVBRequestandRealizationVb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyRateNonPO",
                table: "RealizationVbs",
                newName: "CurrencyRate");

            migrationBuilder.RenameColumn(
                name: "CurrencyCodeNonPO",
                table: "RealizationVbs",
                newName: "CurrencyCode");

            migrationBuilder.AddColumn<int>(
                name: "UnitDivisionId",
                table: "VbRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitDivisionName",
                table: "VbRequests",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsageVBRequest",
                table: "RealizationVbs",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifiedName",
                table: "RealizationVbs",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitDivisionId",
                table: "VbRequests");

            migrationBuilder.DropColumn(
                name: "UnitDivisionName",
                table: "VbRequests");

            migrationBuilder.DropColumn(
                name: "UsageVBRequest",
                table: "RealizationVbs");

            migrationBuilder.DropColumn(
                name: "VerifiedName",
                table: "RealizationVbs");

            migrationBuilder.RenameColumn(
                name: "CurrencyRate",
                table: "RealizationVbs",
                newName: "CurrencyRateNonPO");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "RealizationVbs",
                newName: "CurrencyCodeNonPO");
        }
    }
}
