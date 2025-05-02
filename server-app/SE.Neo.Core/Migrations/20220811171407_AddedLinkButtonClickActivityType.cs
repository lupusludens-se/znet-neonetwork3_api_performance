using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedLinkButtonClickActivityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activity_Type",
                columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
                values: new object[] { 31, null, null, null, "Link button click", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 31);
        }
    }
}
