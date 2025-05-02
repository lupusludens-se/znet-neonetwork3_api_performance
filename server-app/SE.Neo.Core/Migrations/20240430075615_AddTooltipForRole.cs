using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddTooltipForRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 17, null, null, null, "Manage Company Users", null },
                    { 18, null, null, null, "Manage Own Company", null },
                    { 19, null, null, null, "Manage company projects", null },
                    { 20, null, null, null, "View company Messages", null }
                });

            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission](Role_Id,Permission_Id) VALUES " +
                "(6, 17), (6, 18), (6, 19), (6, 20), (6, 9)");

            migrationBuilder.AddColumn<string>(
                name: "ToolTip",
                table: "Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 20);

            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission] WHERE [Role_Id] = 6");

            migrationBuilder.DropColumn(
               name: "ToolTip",
               table: "Role");
        }
    }
}
