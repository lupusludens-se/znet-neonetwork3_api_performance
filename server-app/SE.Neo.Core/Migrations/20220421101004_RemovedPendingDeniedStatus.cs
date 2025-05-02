using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class RemovedPendingDeniedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 1,
                column: "User_Status_Name",
                value: "Onboard");

            migrationBuilder.UpdateData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 2,
                column: "User_Status_Name",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 3,
                column: "User_Status_Name",
                value: "Inactive");

            migrationBuilder.UpdateData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 4,
                column: "User_Status_Name",
                value: "Deleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 1,
                column: "User_Status_Name",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 2,
                column: "User_Status_Name",
                value: "Denied");

            migrationBuilder.UpdateData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 3,
                column: "User_Status_Name",
                value: "Onboard");

            migrationBuilder.UpdateData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 4,
                column: "User_Status_Name",
                value: "Active");

            migrationBuilder.InsertData(
                table: "User_Status",
                columns: new[] { "User_Status_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "User_Status_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 5, null, null, null, "Inactive", null },
                    { 6, null, null, null, "Deleted", null }
                });
        }
    }
}
