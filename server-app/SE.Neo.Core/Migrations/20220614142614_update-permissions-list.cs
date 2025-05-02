using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updatepermissionslist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission]; DELETE FROM [dbo].[User_Permission]");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 4,
                column: "Permission_Name",
                value: "Data Sync");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Event Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 6,
                column: "Permission_Name",
                value: "Messages All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 7,
                column: "Permission_Name",
                value: "Project Catalog View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 8,
                column: "Permission_Name",
                value: "Project Management All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 9,
                column: "Permission_Name",
                value: "Project Management Own");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 10,
                column: "Permission_Name",
                value: "Send Quote");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Tool Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Forum Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "User Access Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "User Profile Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "Email Alert Management");

            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission](Role_Id,Permission_Id) VALUES " +
                "(1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 6), (1, 7), (2, 7), (4, 7), (1, 8)," +
                " (1, 9), (3, 9), (4, 9), (1, 10), (2, 10), (4, 10), (1, 11), (1, 12), (1, 13), (1, 14), (1,15)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission]; DELETE FROM [dbo].[User_Permission]");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 4,
                column: "Permission_Name",
                value: "Dashboard Q3 Pricing Report");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Data Sync");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 6,
                column: "Permission_Name",
                value: "Event Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 7,
                column: "Permission_Name",
                value: "Messages All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 8,
                column: "Permission_Name",
                value: "Project Catalog View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 9,
                column: "Permission_Name",
                value: "Project Management All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 10,
                column: "Permission_Name",
                value: "Project Management Own");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Send Quote");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Tool Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "Forum Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "User Access Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "User Profile Edit");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[] { 16, null, null, null, "Email Alert Management", null });
        }
    }
}
