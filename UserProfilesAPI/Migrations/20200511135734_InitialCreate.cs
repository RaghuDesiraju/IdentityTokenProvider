using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserProfilesAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthUser",
                columns: table => new
                {
                    AuthUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 10, nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUser", x => x.AuthUserId);
                });

            migrationBuilder.CreateTable(
                name: "ClientCredential",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<string>(nullable: true),
                    ClientSecretHash = table.Column<string>(nullable: true),
                    AllowedGrantTypes = table.Column<string>(nullable: true),
                    AllowOfflineAccess = table.Column<int>(nullable: true),
                    RefreshTokenUsage = table.Column<int>(nullable: false),
                    AccessTokenLifetime = table.Column<int>(nullable: false),
                    RefreshTokenExpiration = table.Column<int>(nullable: false),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCredential", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "UserandRole",
                columns: table => new
                {
                    AuthUserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserandRole", x => new { x.AuthUserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserandRole_AuthUser_AuthUserId",
                        column: x => x.AuthUserId,
                        principalTable: "AuthUser",
                        principalColumn: "AuthUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserandRole_UserRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserandRole_RoleId",
                table: "UserandRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientCredential");

            migrationBuilder.DropTable(
                name: "UserandRole");

            migrationBuilder.DropTable(
                name: "AuthUser");

            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
