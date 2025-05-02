using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class ActivityTypeUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 2,
                column: "Activitity_Type_Name",
                value: "Search filter apply");

            migrationBuilder.InsertData(
                table: "Activity_Type",
                columns: new[] { "Activitity_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 4, null, null, null, "Company profile open", null },
                    { 5, null, null, null, "Follow a company", null },
                    { 6, null, null, null, "Contact Provider button click", null },
                    { 7, null, null, null, "Nevermind button click during contacting provider", null },
                    { 8, null, null, null, "Confirm button click during contacting provider", null },
                    { 9, null, null, null, "Event details open", null },
                    { 10, null, null, null, "Attending yes/no button click", null },
                    { 11, null, null, null, "First click on an element on dashboard", null },
                    { 12, null, null, null, "Start following a user", null },
                    { 13, null, null, null, "Start following a forum", null },
                    { 14, null, null, null, "Add a forum to saved items", null },
                    { 15, null, null, null, "Forum open", null },
                    { 16, null, null, null, "Click on a link", null },
                    { 17, null, null, null, "Add a project to saved items", null },
                    { 18, null, null, null, "Project details open", null },
                    { 19, null, null, null, "Click on a tool", null },
                    { 20, null, null, null, "User profile open", null },
                    { 21, null, null, null, "Your discission filter apply", null },
                    { 22, null, null, null, "Start a Discussion button click", null },
                    { 23, null, null, null, "For you filter apply", null },
                    { 24, null, null, null, "Saved filter apply", null },
                    { 25, null, null, null, "View Map button click", null },
                    { 26, null, null, null, "Connect with NEO button click", null },
                    { 27, null, null, null, "New Project button click", null },
                    { 28, null, null, null, "Response on a forum message", null },
                    { 29, null, null, null, "Forum message like button click", null },
                    { 30, null, null, null, "Article open", null },
                    { 31, null, null, null, "About Technologies/Solutions button click", null },
                    { 32, null, null, null, "View all button click", null },
                    { 33, null, null, null, "Project resource click", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Activity_Type",
                keyColumn: "Activitity_Type_Id",
                keyValue: 30);

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
                keyValue: 2,
                column: "Activitity_Type_Name",
                value: "Local Search filter apply");
        }
    }
}
