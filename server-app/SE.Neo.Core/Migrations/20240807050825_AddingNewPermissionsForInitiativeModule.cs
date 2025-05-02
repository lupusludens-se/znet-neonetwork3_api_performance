using Microsoft.EntityFrameworkCore.Migrations;
using SE.Neo.Core.Entities;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddingNewPermissionsForInitiativeModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM [dbo].[Permission] WHERE [Permission_Id] = 21) BEGIN " +
                "INSERT INTO [dbo].[Permission] ([Permission_Id], [Permission_Name]) VALUES (21, 'Initiative Management Own') END");

            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM [dbo].[Permission] WHERE [Permission_Id] = 22) BEGIN " +
                "INSERT INTO [dbo].[Permission] ([Permission_Id], [Permission_Name]) VALUES (22, 'Initiative Management All') END");

            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (2, 21)");

            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission] ([Role_Id], [Permission_Id]) VALUES (1, 22)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission] WHERE [Permission_Id] = 21; " +
                "DELETE FROM [dbo].[User_Permission] WHERE [Permission_Id] = 21; " +
                "DELETE FROM [dbo].[Permission] WHERE [Permission_Id] = 21" );

            migrationBuilder.Sql("DELETE FROM [dbo].[Role_Permission] WHERE [Permission_Id] = 22; " +
                "DELETE FROM [dbo].[User_Permission] WHERE [Permission_Id] = 22; " +
                "DELETE FROM [dbo].[Permission] WHERE [Permission_Id] = 22");
        }
    }
}
