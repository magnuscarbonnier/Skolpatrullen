using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class FileDeleteBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_ProfilePictureId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ProfilePictureId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_ProfilePictureId",
                table: "Users",
                column: "ProfilePictureId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_ProfilePictureId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ProfilePictureId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_ProfilePictureId",
                table: "Users",
                column: "ProfilePictureId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
