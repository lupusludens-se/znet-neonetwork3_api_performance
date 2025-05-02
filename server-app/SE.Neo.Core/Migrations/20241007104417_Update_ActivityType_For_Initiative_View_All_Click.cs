using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class Update_ActivityType_For_Initiative_View_All_Click : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 41,
                column: "Activitity_Type_Name",
                value: "Initiative View All Click");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 41,
                column: "Activitity_Type_Name",
                value: "Initiative View All Learn Clcik");
        }
    }
}
