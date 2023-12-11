using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class UpdateVBRealizationChangeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeVBNonPO",
                table: "RealizationVbs",
                newName: "TypeWithOrWithoutVB");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeWithOrWithoutVB",
                table: "RealizationVbs",
                newName: "TypeVBNonPO");
        }
    }
}
