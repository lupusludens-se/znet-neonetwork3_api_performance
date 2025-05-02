using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class add_responsibility_user_profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_Responsibility_Id",
                table: "User_Profile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Responsibility",
                columns: table => new
                {
                    Responsibility_Id = table.Column<int>(type: "int", nullable: false),
                    Responsibility_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsibility", x => x.Responsibility_Id);
                });

            migrationBuilder.InsertData(
                table: "Heard_Via",
                columns: new[] { "Heard_Via_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Heard_Via_Name", "Updated_User_Id" },
                values: new object[] { 15, null, null, null, "I am an employee", null });

            migrationBuilder.InsertData(
                table: "Responsibility",
                columns: new[] { "Responsibility_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Responsibility_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Sustainability", null },
                    { 2, null, null, null, "Procurement", null },
                    { 3, null, null, null, "Buildings", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_User_Responsibility_Id",
                table: "User_Profile",
                column: "User_Responsibility_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Responsibility_User_Responsibility_Id",
                table: "User_Profile",
                column: "User_Responsibility_Id",
                principalTable: "Responsibility",
                principalColumn: "Responsibility_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Responsibility_User_Responsibility_Id",
                table: "User_Profile");

            migrationBuilder.DropTable(
                name: "Responsibility");

            migrationBuilder.DropIndex(
                name: "IX_User_Profile_User_Responsibility_Id",
                table: "User_Profile");

            migrationBuilder.DeleteData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 15);

            migrationBuilder.DropColumn(
                name: "User_Responsibility_Id",
                table: "User_Profile");
        }
    }
}
