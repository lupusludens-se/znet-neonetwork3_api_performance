using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class ActivityLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Page_Name",
                table: "Activity");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Activity",
                newName: "Activity_Type");

            migrationBuilder.AddColumn<int>(
                name: "Activity_Page",
                table: "Activity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Activity_Location",
                columns: table => new
                {
                    Activitity_Location_Id = table.Column<int>(type: "int", nullable: false),
                    Activitity_Location_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_Location", x => x.Activitity_Location_Id);
                });

            migrationBuilder.InsertData(
                table: "Activity_Location",
                columns: new[] { "Activitity_Location_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Location_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Admin page", null },
                    { 2, null, null, null, "Admit users", null },
                    { 3, null, null, null, "Review user", null },
                    { 4, null, null, null, "User management", null },
                    { 5, null, null, null, "Add user", null },
                    { 6, null, null, null, "Edit user", null },
                    { 7, null, null, null, "Company management", null },
                    { 8, null, null, null, "Add company", null },
                    { 9, null, null, null, "Edit company", null },
                    { 10, null, null, null, "Group management", null },
                    { 11, null, null, null, "Add group", null },
                    { 12, null, null, null, "Edit group", null },
                    { 13, null, null, null, "Learn page", null },
                    { 14, null, null, null, "Learn details", null },
                    { 15, null, null, null, "Events page", null },
                    { 16, null, null, null, "Event details", null },
                    { 17, null, null, null, "Add event", null },
                    { 18, null, null, null, "Forums page", null },
                    { 19, null, null, null, "Forum details", null },
                    { 20, null, null, null, "Add forum", null },
                    { 21, null, null, null, "Tool management", null },
                    { 22, null, null, null, "Tools page", null },
                    { 23, null, null, null, "Add tool", null },
                    { 24, null, null, null, "Edit tool", null },
                    { 25, null, null, null, "View tool", null },
                    { 26, null, null, null, "Email alert settings", null },
                    { 27, null, null, null, "Announcement management", null },
                    { 28, null, null, null, "Add announcement", null },
                    { 29, null, null, null, "Edit announcement", null },
                    { 30, null, null, null, "Dashboard page", null },
                    { 31, null, null, null, "Project catalog", null },
                    { 32, null, null, null, "Project details", null },
                    { 33, null, null, null, "Project library", null },
                    { 34, null, null, null, "Add project", null },
                    { 35, null, null, null, "Edit project", null },
                    { 36, null, null, null, "Community page", null },
                    { 37, null, null, null, "Search result page", null },
                    { 38, null, null, null, "Notifications page", null },
                    { 39, null, null, null, "Messages page", null },
                    { 40, null, null, null, "Add message", null },
                    { 41, null, null, null, "View message", null },
                    { 42, null, null, null, "Saved content page", null }
                });

            migrationBuilder.InsertData(
                table: "Activity_Location",
                columns: new[] { "Activitity_Location_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Activitity_Location_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 43, null, null, null, "Topic page", null },
                    { 44, null, null, null, "View user profile", null },
                    { 45, null, null, null, "Edit user profile", null },
                    { 46, null, null, null, "View company profile", null },
                    { 47, null, null, null, "Account settings", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity_Location");

            migrationBuilder.DropColumn(
                name: "Activity_Page",
                table: "Activity");

            migrationBuilder.RenameColumn(
                name: "Activity_Type",
                table: "Activity",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Page_Name",
                table: "Activity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
