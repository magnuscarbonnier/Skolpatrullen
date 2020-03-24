using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class FileInAssignmentFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentFiles_Users_UserId",
                table: "AssignmentFiles");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AssignmentFiles",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentFiles_FileId",
                table: "AssignmentFiles",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentFiles_Files_FileId",
                table: "AssignmentFiles",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentFiles_Users_UserId",
                table: "AssignmentFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentFiles_Files_FileId",
                table: "AssignmentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentFiles_Users_UserId",
                table: "AssignmentFiles");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentFiles_FileId",
                table: "AssignmentFiles");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AssignmentFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentFiles_Users_UserId",
                table: "AssignmentFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
