using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddActivityTypeForMenuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "Activity_Location",
                columns: new[] { "Activitity_Location_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Location_Name", "Updated_User_Id" },
                values: new object[] { 51, null, null, null, "Manage SP Profile Page", null });

            migrationBuilder.InsertData(
                table: "Nav_Menu_Item",
                columns: new[] { "Nav_Menu_Item_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Nav_Menu_Item_Name", "Updated_User_Id" },
                values: new object[] { 11, null, null, null, "ManageProfile", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Nav_Menu_Item",
                keyColumn: "Nav_Menu_Item_Id",
                keyValue: 11);
        }
    }
}
