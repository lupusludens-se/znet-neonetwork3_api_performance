using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class update_heart_via_enum_list : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[User_Pending] SET [User_Pending_Heard_Via_Id] = 9 WHERE [User_Pending_Heard_Via_Id]>9 " +
                "UPDATE [dbo].[User] SET [User_Heard_Via_Id] = 9 WHERE [User_Heard_Via_Id]>9");

            migrationBuilder.DeleteData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 15);

            migrationBuilder.AlterColumn<int>(
                name: "User_Heard_Via_Id",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 9,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 14);

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 2,
                column: "Heard_Via_Name",
                value: "Co-Worker");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 3,
                column: "Heard_Via_Name",
                value: "NEO Network Member");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 4,
                column: "Heard_Via_Name",
                value: "News/Article");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 5,
                column: "Heard_Via_Name",
                value: "Referrend by Schneider Electric Contact");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 6,
                column: "Heard_Via_Name",
                value: "Social Media");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 7,
                column: "Heard_Via_Name",
                value: "Web Search");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 8,
                column: "Heard_Via_Name",
                value: "I am an Employee");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 9,
                column: "Heard_Via_Name",
                value: "Other");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "User_Heard_Via_Id",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 14,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 9);

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 2,
                column: "Heard_Via_Name",
                value: "Co-worker");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 3,
                column: "Heard_Via_Name",
                value: "Email");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 4,
                column: "Heard_Via_Name",
                value: "NEO Network Member");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 5,
                column: "Heard_Via_Name",
                value: "News/Article");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 6,
                column: "Heard_Via_Name",
                value: "Referrend by Schneider Electric Client Manager");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 7,
                column: "Heard_Via_Name",
                value: "Referrend by other Schneider Electric employee");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 8,
                column: "Heard_Via_Name",
                value: "Renewable Energy Developer/Energy Company");

            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 9,
                column: "Heard_Via_Name",
                value: "Schneider Electric's Perspectives Summit");

            migrationBuilder.InsertData(
                table: "Heard_Via",
                columns: new[] { "Heard_Via_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Heard_Via_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 10, null, null, null, "Social Media", null },
                    { 11, null, null, null, "Television", null },
                    { 12, null, null, null, "The World Bank", null },
                    { 13, null, null, null, "Web Search", null },
                    { 14, null, null, null, "Other", null },
                    { 15, null, null, null, "I am an Employee", null }
                });
        }
    }
}