using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InsertBatteryStorageForPPAProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO[dbo].[CMS_Category_Technology]" +
                                   "([CMS_Category_Id]" +
                                   ",[CMS_Technology_Id]" +
                                   ",[Created_User_Id]" +
                                   ",[Updated_User_Id]" +
                                   ",[Created_Ts]" +
                                   ",[Last_Change_Ts])" +
                                   "VALUES " +
                                   "(48" +
                                   ", 33" +
                                   ", null" +
                                   ", null" +
                                   ",GETDATE()" +
                                   ",GETDATE())");

            migrationBuilder.Sql("INSERT INTO[dbo].[CMS_Category_Technology]" +
                                   "([CMS_Category_Id]" +
                                   ",[CMS_Technology_Id]" +
                                   ",[Created_User_Id]" +
                                   ",[Updated_User_Id]" +
                                   ",[Created_Ts]" +
                                   ",[Last_Change_Ts])" +
                                   "VALUES " +
                                   "(32" +
                                   ", 33" +
                                   ", null" +
                                   ", null" +
                                   ",GETDATE()" +
                                   ",GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE from [dbo].[CMS_Category_Technology] " +
                                 "WHERE [CMS_Category_Id] in " +
                                 "(32,48) " +
                                 "AND [CMS_Technology_Id] in " +
                                 "(33)");
        }
    }
}
