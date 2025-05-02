using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddInitiativeTypeActivityData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activity_Location",
                columns: new[] { "Activitity_Location_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Location_Name", "Updated_User_Id" },
                values: new object[] { 52, null, null, null, "View Initiative", null });

            migrationBuilder.InsertData(
                table: "Activity_Type",
                columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 38, null, null, null, "Initiative Create Click", null },
                    { 39, null, null, null, "Initiative Details View", null },
                    { 40, null, null, null, "Initiative Sub Step Click", null },
                    { 41, null, null, null, "Initiative View All Learn Clcik", null }
                });

            migrationBuilder.InsertData(
                table: "Dashboard_Click_Element_Action_Type",
                columns: new[] { "Dashboard_Click_Element_Action_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Dashboard_Click_Element_Action_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 42, null, null, null, "Initiative Create Click", null },
                    { 43, null, null, null, "Initiative View", null }
                });

            migrationBuilder.InsertData(
                table: "Dashboard_Resource_View_All_Type",
                columns: new[] { "Dashboard_Resource_View_All_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Dashboard_Resource_View_All_Type_Name", "Updated_User_Id" },
                values: new object[] { 7, null, null, null, "Initiative", null });

        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Dashboard_Click_Element_Action_Type",
                keyColumn: "Dashboard_Click_Element_Action_Type_Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Dashboard_Click_Element_Action_Type",
                keyColumn: "Dashboard_Click_Element_Action_Type_Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Dashboard_Resource_View_All_Type",
                keyColumn: "Dashboard_Resource_View_All_Type_Id",
                keyValue: 7);
        }
    }
}
