using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InsertNavMenuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nav_Menu_Item",
                columns: new[] { "Nav_Menu_Item_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Nav_Menu_Item_Name", "Updated_User_Id" },
                values: new object[] { 10, null, null, null, "Messages", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nav_Menu_Item",
                keyColumn: "Nav_Menu_Item_Id",
                keyValue: 10);
        }
    }
}
