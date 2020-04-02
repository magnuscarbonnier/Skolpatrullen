using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class CourseBlogFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseBlogPost_Courses_CourseId",
                table: "CourseBlogPost");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseBlogPost_Users_UserId",
                table: "CourseBlogPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseBlogPost",
                table: "CourseBlogPost");

            migrationBuilder.RenameTable(
                name: "CourseBlogPost",
                newName: "CourseBlogPosts");

            migrationBuilder.RenameIndex(
                name: "IX_CourseBlogPost_UserId",
                table: "CourseBlogPosts",
                newName: "IX_CourseBlogPosts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseBlogPost_CourseId",
                table: "CourseBlogPosts",
                newName: "IX_CourseBlogPosts_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseBlogPosts",
                table: "CourseBlogPosts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseBlogPosts_Courses_CourseId",
                table: "CourseBlogPosts",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseBlogPosts_Users_UserId",
                table: "CourseBlogPosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseBlogPosts_Courses_CourseId",
                table: "CourseBlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseBlogPosts_Users_UserId",
                table: "CourseBlogPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseBlogPosts",
                table: "CourseBlogPosts");

            migrationBuilder.RenameTable(
                name: "CourseBlogPosts",
                newName: "CourseBlogPost");

            migrationBuilder.RenameIndex(
                name: "IX_CourseBlogPosts_UserId",
                table: "CourseBlogPost",
                newName: "IX_CourseBlogPost_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseBlogPosts_CourseId",
                table: "CourseBlogPost",
                newName: "IX_CourseBlogPost_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseBlogPost",
                table: "CourseBlogPost",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseBlogPost_Courses_CourseId",
                table: "CourseBlogPost",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseBlogPost_Users_UserId",
                table: "CourseBlogPost",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
