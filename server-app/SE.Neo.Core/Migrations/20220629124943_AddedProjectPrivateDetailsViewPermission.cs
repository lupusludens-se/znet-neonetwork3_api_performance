using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedProjectPrivateDetailsViewPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[] { 16, null, null, null, "Project Private Details View", null });

            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission](Role_Id,Permission_Id) VALUES " +
                "(1, 16), (4, 16)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission] WHERE [Permission_Id] = 16; DELETE FROM [dbo].[User_Permission] WHERE [Permission_Id] = 16");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 16);
        }
    }
}
