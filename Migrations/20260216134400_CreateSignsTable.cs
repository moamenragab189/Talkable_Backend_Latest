using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talkable.Migrations
{
    /// <inheritdoc />
    public partial class CreateSignsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Instructor_Info = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Signs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    AnimationPath = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Signs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Users",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Users", x => x.User_Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_CourseFeedback",
                columns: table => new
                {
                    Feedback_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Course_Id = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Created_At = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_CourseFeedback", x => x.Feedback_Id);
                    table.ForeignKey(
                        name: "FK_Tb_CourseFeedback_Tb_Courses_Course_Id",
                        column: x => x.Course_Id,
                        principalTable: "Tb_Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_CourseFeedback_Tb_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Tb_Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_UserCourses",
                columns: table => new
                {
                    UserCourses_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Course_Id = table.Column<int>(type: "int", nullable: false),
                    Progress = table.Column<int>(type: "int", nullable: false),
                    Completed_Lessons = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_UserCourses", x => x.UserCourses_Id);
                    table.ForeignKey(
                        name: "FK_Tb_UserCourses_Tb_Courses_Course_Id",
                        column: x => x.Course_Id,
                        principalTable: "Tb_Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_UserCourses_Tb_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Tb_Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_CourseFeedback_Course_Id",
                table: "Tb_CourseFeedback",
                column: "Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_CourseFeedback_User_Id",
                table: "Tb_CourseFeedback",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_UserCourses_Course_Id",
                table: "Tb_UserCourses",
                column: "Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_UserCourses_User_Id",
                table: "Tb_UserCourses",
                column: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_CourseFeedback");

            migrationBuilder.DropTable(
                name: "Tb_Signs");

            migrationBuilder.DropTable(
                name: "Tb_UserCourses");

            migrationBuilder.DropTable(
                name: "Tb_Courses");

            migrationBuilder.DropTable(
                name: "Tb_Users");
        }
    }
}
