using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddActivityLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activity_Location",
                columns: new[] { "Activitity_Location_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Location_Name", "Updated_User_Id" },
                values: new object[] { 53, null, null, null, "Create Initiative", null });

            migrationBuilder.InsertData(
                table: "Activity_Location",
                columns: new[] { "Activitity_Location_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Location_Name", "Updated_User_Id" },
                values: new object[] { 54, null, null, null, "Initiative View All Recommended And Saved Page", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 54);
        }
    }
}
