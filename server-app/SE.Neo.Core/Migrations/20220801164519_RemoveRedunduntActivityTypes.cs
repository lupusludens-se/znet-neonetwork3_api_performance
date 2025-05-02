using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class RemoveRedunduntActivityTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 33);

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 21,
                column: "Activitity_Type_Name",
                value: "Start a Discussion button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 22,
                column: "Activitity_Type_Name",
                value: "View Map button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 23,
                column: "Activitity_Type_Name",
                value: "Connect with NEO button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 24,
                column: "Activitity_Type_Name",
                value: "New Project button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 25,
                column: "Activitity_Type_Name",
                value: "Response on a forum message");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 26,
                column: "Activitity_Type_Name",
                value: "Forum message like button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 27,
                column: "Activitity_Type_Name",
                value: "Article open");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 28,
                column: "Activitity_Type_Name",
                value: "About Technologies/Solutions button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 29,
                column: "Activitity_Type_Name",
                value: "View all button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 30,
                column: "Activitity_Type_Name",
                value: "Project resource click");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 21,
                column: "Activitity_Type_Name",
                value: "Your discission filter apply");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 22,
                column: "Activitity_Type_Name",
                value: "Start a Discussion button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 23,
                column: "Activitity_Type_Name",
                value: "For you filter apply");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 24,
                column: "Activitity_Type_Name",
                value: "Saved filter apply");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 25,
                column: "Activitity_Type_Name",
                value: "View Map button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 26,
                column: "Activitity_Type_Name",
                value: "Connect with NEO button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 27,
                column: "Activitity_Type_Name",
                value: "New Project button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 28,
                column: "Activitity_Type_Name",
                value: "Response on a forum message");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 29,
                column: "Activitity_Type_Name",
                value: "Forum message like button click");

            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 30,
                column: "Activitity_Type_Name",
                value: "Article open");

            migrationBuilder.InsertData(
                table: "Activity_Type",
                columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 31, null, null, null, "About Technologies/Solutions button click", null },
                    { 32, null, null, null, "View all button click", null },
                    { 33, null, null, null, "Project resource click", null }
                });
        }
    }
}
