using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateCaptilisePermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 19,
                column: "Permission_Name",
                value: "Manage Company Projects");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 20,
                column: "Permission_Name",
                value: "View Company Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 19,
                column: "Permission_Name",
                value: "Manage Company projects");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 20,
                column: "Permission_Name",
                value: "View Company Messages");
        }
    }
}
