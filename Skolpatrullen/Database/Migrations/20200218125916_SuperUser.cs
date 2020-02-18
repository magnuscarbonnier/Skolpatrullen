using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class SuperUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuperUser",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuperUser",
                table: "Users");
        }
    }
}
