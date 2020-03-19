using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class changefromfileextensiontocontenttype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Files",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
