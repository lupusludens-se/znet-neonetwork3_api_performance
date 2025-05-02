using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class usereditpermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[] { 30, null, null, null, "User edit", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 30);
        }
    }
}
