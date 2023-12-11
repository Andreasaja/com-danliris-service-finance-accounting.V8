using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class ModifyPropertyVBPositionAndType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "VBRealizationDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "VBType",
                table: "VBRealizationDocumentExpeditions",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "VBRealizationDocuments");

            migrationBuilder.AlterColumn<string>(
                name: "VBType",
                table: "VBRealizationDocumentExpeditions",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 64);
        }
    }
}
