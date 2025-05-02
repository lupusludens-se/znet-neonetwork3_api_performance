using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedTrackingActivityDictionaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dashboard_Click_Element_Action_Type",
                columns: table => new
                {
                    Dashboard_Click_Element_Action_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Dashboard_Click_Element_Action_Type_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboard_Click_Element_Action_Type", x => x.Dashboard_Click_Element_Action_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Dashboard_Resource_View_All_Type",
                columns: table => new
                {
                    Dashboard_Resource_View_All_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Dashboard_Resource_View_All_Type_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboard_Resource_View_All_Type", x => x.Dashboard_Resource_View_All_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Nav_Menu_Item",
                columns: table => new
                {
                    Nav_Menu_Item_Id = table.Column<int>(type: "int", nullable: false),
                    Nav_Menu_Item_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nav_Menu_Item", x => x.Nav_Menu_Item_Id);
                });

            migrationBuilder.InsertData(
                table: "Dashboard_Click_Element_Action_Type",
                columns: new[] { "Dashboard_Click_Element_Action_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Dashboard_Click_Element_Action_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Messages View", null },
                    { 2, null, null, null, "Report View", null },
                    { 3, null, null, null, "Pinned Tools Customize", null },
                    { 4, null, null, null, "Pinned Tool View", null },
                    { 5, null, null, null, "Pinned Tool Add", null },
                    { 6, null, null, null, "Learn View All", null },
                    { 7, null, null, null, "Learn View", null },
                    { 8, null, null, null, "Learn Save", null },
                    { 9, null, null, null, "Learn Unsave", null },
                    { 10, null, null, null, "Learn Tag Click", null },
                    { 11, null, null, null, "Events View All", null },
                    { 12, null, null, null, "Event View", null },
                    { 13, null, null, null, "Announcement Hide", null },
                    { 14, null, null, null, "Announcement Button Click", null },
                    { 15, null, null, null, "Suggestion Hide", null },
                    { 16, null, null, null, "Suggestion Skip", null },
                    { 17, null, null, null, "Suggestion Take", null },
                    { 18, null, null, null, "Project Catalog Browse", null },
                    { 19, null, null, null, "About Solutions Click", null },
                    { 20, null, null, null, "About Technologies Click", null },
                    { 21, null, null, null, "Forums View All", null },
                    { 22, null, null, null, "Forum View", null },
                    { 23, null, null, null, "Forum Category Click", null },
                    { 24, null, null, null, "Forum Region Click", null },
                    { 25, null, null, null, "Forum Save", null },
                    { 26, null, null, null, "Forum Unsave", null },
                    { 27, null, null, null, "Forum Owner Follow", null },
                    { 28, null, null, null, "Forum Owner View", null },
                    { 29, null, null, null, "Forum Like", null },
                    { 30, null, null, null, "Forum UnLike", null },
                    { 31, null, null, null, "Forum Comment", null },
                    { 32, null, null, null, "Project View", null },
                    { 33, null, null, null, "Project Save", null },
                    { 34, null, null, null, "Project Unsave", null },
                    { 35, null, null, null, "Project Company View", null },
                    { 36, null, null, null, "Project Tag Click", null },
                    { 37, null, null, null, "Company View", null },
                    { 38, null, null, null, "Companies View All", null },
                    { 39, null, null, null, "Companies Browse", null }
                });

            migrationBuilder.InsertData(
                table: "Dashboard_Resource_View_All_Type",
                columns: new[] { "Dashboard_Resource_View_All_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Dashboard_Resource_View_All_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Event", null },
                    { 2, null, null, null, "Forum", null },
                    { 3, null, null, null, "Project", null }
                });

            migrationBuilder.InsertData(
                table: "Dashboard_Resource_View_All_Type",
                columns: new[] { "Dashboard_Resource_View_All_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Dashboard_Resource_View_All_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 4, null, null, null, "Company", null },
                    { 5, null, null, null, "Learn", null },
                    { 6, null, null, null, "Message", null }
                });

            migrationBuilder.InsertData(
                table: "Nav_Menu_Item",
                columns: new[] { "Nav_Menu_Item_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Nav_Menu_Item_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Admin", null },
                    { 2, null, null, null, "Dashboard", null },
                    { 3, null, null, null, "Projects", null },
                    { 4, null, null, null, "ProjectLibrary", null },
                    { 5, null, null, null, "Learn", null },
                    { 6, null, null, null, "Events", null },
                    { 7, null, null, null, "Tools", null },
                    { 8, null, null, null, "Community", null },
                    { 9, null, null, null, "Forum", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dashboard_Click_Element_Action_Type");

            migrationBuilder.DropTable(
                name: "Dashboard_Resource_View_All_Type");

            migrationBuilder.DropTable(
                name: "Nav_Menu_Item");
        }
    }
}
