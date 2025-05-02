using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateManageProfileMenuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 51,
                column: "Activitity_Location_Name",
                value: "Manage");

            migrationBuilder.UpdateData(
                table: "Nav_Menu_Item",
                keyColumn: "Nav_Menu_Item_Id",
                keyValue: 11,
                column: "Nav_Menu_Item_Name",
                value: "Manage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 51,
                column: "Activitity_Location_Name",
                value: "Manage SP Profile Page");

            migrationBuilder.UpdateData(
                table: "Nav_Menu_Item",
                keyColumn: "Nav_Menu_Item_Id",
                keyValue: 11,
                column: "Nav_Menu_Item_Name",
                value: "ManageProfile");
        }
    }
}
