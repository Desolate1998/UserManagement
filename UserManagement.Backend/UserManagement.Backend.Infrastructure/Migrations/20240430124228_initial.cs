using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_Group_Code", x => x.GroupCode);
                });

            migrationBuilder.CreateTable(
                name: "LoginStatusLookup",
                columns: table => new
                {
                    StatusCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("StatusCode", x => x.StatusCode);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionCode);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    EntryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_User_Entry_Id", x => x.EntryId);
                });

            migrationBuilder.CreateTable(
                name: "GroupPermission",
                columns: table => new
                {
                    GroupCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PermissionCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPermission", x => new { x.GroupCode, x.PermissionCode });
                    table.ForeignKey(
                        name: "FK_GroupPermission_Groups_GroupsCode",
                        column: x => x.GroupCode,
                        principalTable: "Groups",
                        principalColumn: "GroupCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPermission_Permissions_PermissionsCode",
                        column: x => x.PermissionCode,
                        principalTable: "Permissions",
                        principalColumn: "PermissionCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => new { x.GroupCode, x.UserId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_GroupsCode",
                        column: x => x.GroupCode,
                        principalTable: "Groups",
                        principalColumn: "GroupCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Users_UsersEntryId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginHistory",
                columns: table => new
                {
                    EntryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusLookupStatusCode = table.Column<string>(type: "nvarchar(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginHistory", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_UserLoginHistory_Status_StatusLookup",
                        column: x => x.StatusLookupStatusCode,
                        principalTable: "LoginStatusLookup",
                        principalColumn: "StatusCode");
                    table.ForeignKey(
                        name: "FK_UserLogin_Users_UsersEntryId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermission_PermissionCode",
                table: "GroupPermission",
                column: "PermissionCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistory_StatusLookupStatusCode",
                table: "UserLoginHistory",
                column: "StatusLookupStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistory_UserId",
                table: "UserLoginHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupPermission");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "UserLoginHistory");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "LoginStatusLookup");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
