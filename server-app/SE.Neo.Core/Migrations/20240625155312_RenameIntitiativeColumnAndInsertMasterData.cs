using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class RenameIntitiativeColumnAndInsertMasterData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScaleIdId",
                table: "Initiative",
                newName: "Scale_Id");

            migrationBuilder.RenameColumn(
                name: "StatusIdId",
                table: "Initiative",
                newName: "Status_Id");

            migrationBuilder.Sql("INSERT INTO [dbo].[Initiative_Status]([Status_Id],[Status_Name],[Created_User_Id],[Updated_User_Id],[Created_Ts],[Last_Change_Ts]) VALUES (1 ,'Active',NULL,NULL,GETDATE() ,GETDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Initiative_Status]([Status_Id],[Status_Name],[Created_User_Id],[Updated_User_Id],[Created_Ts],[Last_Change_Ts]) VALUES (2 ,'Draft',NULL,NULL,GETDATE() ,GETDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Initiative_Status]([Status_Id],[Status_Name],[Created_User_Id],[Updated_User_Id],[Created_Ts],[Last_Change_Ts]) VALUES (3 ,'Deleted',NULL,NULL,GETDATE() ,GETDATE())");

            migrationBuilder.Sql("INSERT INTO [dbo].[Initiative_Scale] ([Initiative_Scale_Id] ,[Name] ,[Created_User_Id] ,[Updated_User_Id] ,[Created_Ts] ,[Last_Change_Ts]) VALUES (1,'State Level (Choose one or more states - US only)',NULL,NULL,GETDATE(),GETDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Initiative_Scale] ([Initiative_Scale_Id] ,[Name] ,[Created_User_Id] ,[Updated_User_Id] ,[Created_Ts] ,[Last_Change_Ts]) VALUES (2,'National (Choose one or more countries)',NULL,NULL,GETDATE(),GETDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Initiative_Scale] ([Initiative_Scale_Id] ,[Name] ,[Created_User_Id] ,[Updated_User_Id] ,[Created_Ts] ,[Last_Change_Ts]) VALUES (3,'Regional (Choose a continent)',NULL,NULL,GETDATE(),GETDATE())");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Scale_Id",
                table: "Initiative",
                newName: "ScaleIdId");

            migrationBuilder.RenameColumn(
                name: "Status_Id",
                table: "Initiative",
                newName: "StatusIdId");

            migrationBuilder.Sql("DELETE FROM [dbo].[Initiative_Scale]");
            migrationBuilder.Sql("DELETE FROM [dbo].[Initiative_Status]");

        }
    }
}
