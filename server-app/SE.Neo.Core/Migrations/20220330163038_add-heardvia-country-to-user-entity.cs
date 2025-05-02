using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addheardviacountrytouserentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Country_Country_Id",
                table: "User_Profile");

            migrationBuilder.DropIndex(
                name: "IX_User_Profile_Country_Id",
                table: "User_Profile");

            migrationBuilder.DropColumn(
                name: "Country_Id",
                table: "User_Profile");

            migrationBuilder.AddColumn<int>(
                name: "Country_Id",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "User_Heard_Via_Id",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 14);

            migrationBuilder.CreateTable(
                name: "Heard_Via",
                columns: table => new
                {
                    Heard_Via_Id = table.Column<int>(type: "int", nullable: false),
                    Heard_Via_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heard_Via", x => x.Heard_Via_Id);
                });

            migrationBuilder.InsertData(
                table: "Heard_Via",
                columns: new[] { "Heard_Via_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Heard_Via_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Conference/Event", null },
                    { 2, null, null, null, "Co-worker", null },
                    { 3, null, null, null, "Email", null },
                    { 4, null, null, null, "NEO Network Member", null },
                    { 5, null, null, null, "News/Article", null },
                    { 6, null, null, null, "Referrend by Schneider Electric Client Manager", null },
                    { 7, null, null, null, "Referrend by other Schneider Electric employee", null },
                    { 8, null, null, null, "Renewable Energy Developer/Energy Company", null },
                    { 9, null, null, null, "Schneider Electric's Perspectives Summit", null },
                    { 10, null, null, null, "Social Media", null },
                    { 11, null, null, null, "Television", null },
                    { 12, null, null, null, "The World Bank", null },
                    { 13, null, null, null, "Web Search", null },
                    { 14, null, null, null, "Other", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Country_Id",
                table: "User",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_User_Heard_Via_Id",
                table: "User",
                column: "User_Heard_Via_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Country_Country_Id",
                table: "User",
                column: "Country_Id",
                principalTable: "Country",
                principalColumn: "Country_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Heard_Via_User_Heard_Via_Id",
                table: "User",
                column: "User_Heard_Via_Id",
                principalTable: "Heard_Via",
                principalColumn: "Heard_Via_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Country_Country_Id",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Heard_Via_User_Heard_Via_Id",
                table: "User");

            migrationBuilder.DropTable(
                name: "Heard_Via");

            migrationBuilder.DropIndex(
                name: "IX_User_Country_Id",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_User_Heard_Via_Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Country_Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "User_Heard_Via_Id",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "Country_Id",
                table: "User_Profile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Country_Id",
                table: "User_Profile",
                column: "Country_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Country_Country_Id",
                table: "User_Profile",
                column: "Country_Id",
                principalTable: "Country",
                principalColumn: "Country_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
