using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class removehashtag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashTags",
                table: "CourseBlogPosts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashTags",
                table: "CourseBlogPosts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
