using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddExpiredUserStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User_Status",
                columns: new[] { "User_Status_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "User_Status_Name", "Updated_User_Id" },
                values: new object[] { 5, null, null, null, "Expired", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User_Status",
                keyColumn: "User_Status_Id",
                keyValue: 5);
        }
    }
}
