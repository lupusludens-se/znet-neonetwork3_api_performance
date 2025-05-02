using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UrlLinksFollowersIsApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn_Url",
                table: "User_Profile",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Job_Title",
                table: "User_Profile",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "User_Profile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Approved",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "User_Follower",
                columns: table => new
                {
                    User_Follower_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Follower_Id = table.Column<int>(type: "int", nullable: false),
                    Followed_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Follower", x => x.User_Follower_Id);
                    table.ForeignKey(
                        name: "FK_User_Follower_User_Followed_Id",
                        column: x => x.Followed_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Follower_User_Follower_Id",
                        column: x => x.Follower_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Profile_Url_Link",
                columns: table => new
                {
                    Url_Link_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url_Link = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Url_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profile_Url_Link", x => x.Url_Link_Id);
                    table.ForeignKey(
                        name: "FK_User_Profile_Url_Link_User_Profile_User_Profile_Id",
                        column: x => x.User_Profile_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Follower_Followed_Id",
                table: "User_Follower",
                column: "Followed_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Follower_Follower_Id",
                table: "User_Follower",
                column: "Follower_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Url_Link_User_Profile_Id",
                table: "User_Profile_Url_Link",
                column: "User_Profile_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Follower");

            migrationBuilder.DropTable(
                name: "User_Profile_Url_Link");

            migrationBuilder.DropColumn(
                name: "Is_Approved",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn_Url",
                table: "User_Profile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "Job_Title",
                table: "User_Profile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "User_Profile",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
