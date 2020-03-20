using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class removecoursefilename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CourseFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CourseFiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
