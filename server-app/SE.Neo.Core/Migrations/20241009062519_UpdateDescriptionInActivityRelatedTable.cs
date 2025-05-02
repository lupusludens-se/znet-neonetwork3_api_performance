using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateDescriptionInActivityRelatedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 54,
                column: "Activitity_Location_Name",
                value: "Initiative Manage Module Page");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 41,
                column: "Activitity_Type_Name",
                value: "Initiative Module View All Click");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity_Location",
                keyColumn: "Activitity_Location_Id",
                keyValue: 54,
                column: "Activitity_Location_Name",
                value: "Initiative View All Recommended And Saved Page");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 41,
                column: "Activitity_Type_Name",
                value: "Initiative View All Click");
        }
    }
}
