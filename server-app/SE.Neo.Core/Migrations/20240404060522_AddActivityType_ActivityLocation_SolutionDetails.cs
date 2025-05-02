using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddActivityType_ActivityLocation_SolutionDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activity_Location",
                columns: new[] { "Activitity_Location_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Location_Name", "Updated_User_Id" },
                values: new object[] { 49, null, null, null, "Solution Details Page", null });

            migrationBuilder.InsertData(
                table: "Activity_Type",
                columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
                values: new object[] { 36, null, null, null, "Solution Details View", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 36);
        }
    }
}
