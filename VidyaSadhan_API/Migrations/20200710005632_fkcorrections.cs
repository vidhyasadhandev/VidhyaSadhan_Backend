using Microsoft.EntityFrameworkCore.Migrations;

namespace VidyaSadhan_API.Migrations
{
    public partial class fkcorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_AspNetUsers_Fk_InsUser",
                table: "Instructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_Fk_InsStudent",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_Fk_InsStudent",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Instructor_Fk_InsUser",
                table: "Instructor");

            migrationBuilder.DropColumn(
                name: "Fk_InsStudent",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Fk_InsUser",
                table: "Instructor");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_AspNetUsers_UserId",
                table: "Instructor",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_UserId",
                table: "Student",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_AspNetUsers_UserId",
                table: "Instructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_UserId",
                table: "Student");

            migrationBuilder.AddColumn<string>(
                name: "Fk_InsStudent",
                table: "Student",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fk_InsUser",
                table: "Instructor",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_Fk_InsStudent",
                table: "Student",
                column: "Fk_InsStudent");

            migrationBuilder.CreateIndex(
                name: "IX_Instructor_Fk_InsUser",
                table: "Instructor",
                column: "Fk_InsUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_AspNetUsers_Fk_InsUser",
                table: "Instructor",
                column: "Fk_InsUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_Fk_InsStudent",
                table: "Student",
                column: "Fk_InsStudent",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
