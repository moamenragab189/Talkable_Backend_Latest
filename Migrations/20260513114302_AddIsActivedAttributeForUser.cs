using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talkable.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActivedAttributeForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tb_OTP_UserId",
                table: "Tb_OTP");

            migrationBuilder.AddColumn<bool>(
                name: "IsActived",
                table: "Tb_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Tb_OTP_UserId",
                table: "Tb_OTP",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tb_OTP_UserId",
                table: "Tb_OTP");

            migrationBuilder.DropColumn(
                name: "IsActived",
                table: "Tb_Users");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_OTP_UserId",
                table: "Tb_OTP",
                column: "UserId",
                unique: true);
        }
    }
}
