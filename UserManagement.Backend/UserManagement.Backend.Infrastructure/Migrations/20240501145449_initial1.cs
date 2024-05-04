using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogin_Users_UsersEntryId",
                table: "UserLoginHistory");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UserLoginHistory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogin_Users_UsersEntryId",
                table: "UserLoginHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "EntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogin_Users_UsersEntryId",
                table: "UserLoginHistory");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UserLoginHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogin_Users_UsersEntryId",
                table: "UserLoginHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
