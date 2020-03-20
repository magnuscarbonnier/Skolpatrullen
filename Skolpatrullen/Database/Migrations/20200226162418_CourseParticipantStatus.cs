using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class CourseParticipantStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CourseParticipants",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CourseParticipants");
        }
    }
}
