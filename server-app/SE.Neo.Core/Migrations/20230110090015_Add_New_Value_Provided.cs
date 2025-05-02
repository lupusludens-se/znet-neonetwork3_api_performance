using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class Add_New_Value_Provided : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Value_Provided",
                columns: new[] { "Value_Provided_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Value_Provided_Name", "Updated_User_Id" },
                values: new object[] { 11, null, null, null, "Greenhouse Gas Emission Reduction Offset", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 11);
        }
    }
}
