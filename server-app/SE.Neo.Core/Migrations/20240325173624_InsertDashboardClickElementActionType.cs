using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InsertDashboardClickElementActionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Dashboard_Click_Element_Action_Type",
                columns: new[] { "Dashboard_Click_Element_Action_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Dashboard_Click_Element_Action_Type_Name", "Updated_User_Id" },
                values: new object[] { 41, null, null, null, "Project Discoverability Item View", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dashboard_Click_Element_Action_Type",
                keyColumn: "Dashboard_Click_Element_Action_Type_Id",
                keyValue: 41);
        }
    }
}
