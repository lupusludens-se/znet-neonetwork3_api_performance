using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using SE.Neo.Core.Entities;
using StackExchange.Redis;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class Update_Event_Tool_Article_User_RoleId_For_SP_Admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF(EXISTS(SELECT 1 FROM [dbo].[Event_Invited_Role])) "+
            "INSERT INTO[dbo].[Event_Invited_Role]([Role_Id], [Event_Id], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) "+
            "SELECT 6 AS[Role_Id], [Event_Id], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts] FROM[dbo].[Event_Invited_Role] WHERE Role_Id = 3 "+

            "IF(EXISTS(SELECT 1 FROM[dbo].[CMS_Article_Role])) "+
            "INSERT INTO[dbo].[CMS_Article_Role]([CMS_Article_Id], [Role_Id], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts]) "+
            "SELECT[CMS_Article_Id], 6 AS[Role_Id], [Created_User_Id], [Updated_User_Id], [Created_Ts], [Last_Change_Ts] FROM[dbo].[CMS_Article_Role] WHERE Role_Id = 3 "+

            "IF(EXISTS(SELECT 1 FROM[dbo].[Tool_Role])) "+
            "INSERT INTO[dbo].[Tool_Role]([Tool_Id], [Role_Id], [Created_Ts], [Created_User_Id], [Last_Change_Ts], [Updated_User_Id]) "+
            "SELECT[Tool_Id], 6 AS[Role_Id], [Created_Ts], [Created_User_Id], [Last_Change_Ts], [Updated_User_Id]  FROM[dbo].[Tool_Role] WHERE Role_Id = 3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF(EXISTS(SELECT 1 FROM [dbo].[Event_Invited_Role] WHERE [Role_Id] = 6)) "+
            "DELETE FROM [dbo].[Event_Invited_Role] WHERE [Role_Id] = 6 "+
            
            "IF(EXISTS(SELECT 1 FROM [dbo].[CMS_Article_Role] WHERE [Role_Id] = 6)) "+
            "DELETE FROM [dbo].[CMS_Article_Role] WHERE [Role_Id] = 6 "+
    
            "IF(EXISTS(SELECT 1 FROM [dbo].[Tool_Role] WHERE [Role_Id] = 6)) "+
            "DELETE FROM [dbo].[Tool_Role] WHERE [Role_Id] = 6");
        }
    }
}
