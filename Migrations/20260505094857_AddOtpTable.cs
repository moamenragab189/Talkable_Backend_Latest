using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talkable.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_OTP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_OTP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_OTP_Tb_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tb_Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_OTP_UserId",
                table: "Tb_OTP",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_OTP");
        }
    }
}
