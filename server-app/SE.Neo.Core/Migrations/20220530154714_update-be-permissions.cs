using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updatebepermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission]; DELETE FROM [dbo].[User_Permission]");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 16);

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

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 33);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 1,
                column: "Permission_Name",
                value: "Admin All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 2,
                column: "Permission_Name",
                value: "Announcement Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 3,
                column: "Permission_Name",
                value: "Company Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 4,
                column: "Permission_Name",
                value: "Dashboard Q3 Pricing Report");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Data Sync");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 6,
                column: "Permission_Name",
                value: "Event Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 7,
                column: "Permission_Name",
                value: "Messages All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 8,
                column: "Permission_Name",
                value: "Project Catalog View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 9,
                column: "Permission_Name",
                value: "Project Management All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 10,
                column: "Permission_Name",
                value: "Project Management Own");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Send Quote");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Tool Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "Topic Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "User Access Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "User Profile Edit");

            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Role] ON " +
                "IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE [Role_Id] = 1) BEGIN INSERT INTO [dbo].[Role] ([Role_Id], [Role_Name], [Is_Special]) VALUES (1, 'Admin', 1) END " +
                "IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE [Role_Id] = 2) BEGIN INSERT INTO [dbo].[Role] ([Role_Id], [Role_Name], [Is_Special]) VALUES (2, 'Corporation', 1) END " +
                "IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE [Role_Id] = 3) BEGIN INSERT INTO [dbo].[Role] ([Role_Id], [Role_Name], [Is_Special]) VALUES (3, 'Solution Provider', 1) END " +
                "IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE [Role_Id] = 4) BEGIN INSERT INTO [dbo].[Role] ([Role_Id], [Role_Name], [Is_Special]) VALUES (4, 'Internal SE', 1) END " +
                "IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE [Role_Id] = 5) BEGIN INSERT INTO [dbo].[Role] ([Role_Id], [Role_Name], [Is_Special]) VALUES (5, 'All', 1) END " +
                "SET IDENTITY_INSERT [dbo].[Role] OFF");

            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission](Role_Id,Permission_Id) VALUES "+
                "(1, 1), (1, 2), (1, 3), (3, 4), (1, 5), (1, 6), (1, 7), (1, 8), (2, 8), (4, 8), (1, 9),"+
                " (1, 10), (3, 10), (4, 10), (1, 11), (2, 11), (4, 11), (1, 12), (1, 13), (1, 14), (1, 15)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission]; DELETE FROM [dbo].[User_Permission]");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 1,
                column: "Permission_Name",
                value: "Account settings edit others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 2,
                column: "Permission_Name",
                value: "Announcement manage");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 3,
                column: "Permission_Name",
                value: "Comment delete others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 4,
                column: "Permission_Name",
                value: "Comment pin");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Company management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 6,
                column: "Permission_Name",
                value: "Event create");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 7,
                column: "Permission_Name",
                value: "Event delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 8,
                column: "Permission_Name",
                value: "Event edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 9,
                column: "Permission_Name",
                value: "Event save draft");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 10,
                column: "Permission_Name",
                value: "Project add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Project edit published by me");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Project edit published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "Project save draft");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "Project mark as favorite");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "Project view all");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 16, null, null, null, "Provider contact", null },
                    { 17, null, null, null, "Tool add", null },
                    { 18, null, null, null, "Tool delete", null },
                    { 19, null, null, null, "Tool edit", null },
                    { 20, null, null, null, "Tool view all", null },
                    { 21, null, null, null, "Topic create private", null },
                    { 22, null, null, null, "Topic delete published by others", null },
                    { 23, null, null, null, "Topic edit published by others", null },
                    { 24, null, null, null, "Topic pin", null },
                    { 25, null, null, null, "User add", null },
                    { 26, null, null, null, "Topic view private", null },
                    { 27, null, null, null, "User export", null },
                    { 28, null, null, null, "User edit", null },
                    { 29, null, null, null, "User admit", null },
                    { 30, null, null, null, "Conversation view all", null },
                    { 31, null, null, null, "Conversation create group", null },
                    { 32, null, null, null, "Event view all", null },
                    { 33, null, null, null, "Data sync", null }
                });
        }
    }
}
