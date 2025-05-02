using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddSystemOwnerRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Role] ON " +
            "IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE [Role_Id] = 7) BEGIN INSERT INTO [dbo].[Role] ([Role_Id], [Role_Name], [Is_Special], [CMS_Role_Id]) VALUES (7, 'System Owner', 1, 29) END " +
            "SET IDENTITY_INSERT [dbo].[Role] OFF");

            migrationBuilder.InsertData(
               table: "Permission",
               columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
               values: new object[] { 23, null, null, null, "Tier Management", null });

            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 1)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 2)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 3)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 4)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 5)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 6)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 7)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 8)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 9)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 10)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 11)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 12)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 13)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 14)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 15)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 16)");
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 22)");
            
            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (7, 23)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission] WHERE [Role_Id] = 7; "); 

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 23);

            migrationBuilder.Sql("DELETE FROM [dbo].[Role] WHERE [Role_Id] = 7");
        }
    }
}
