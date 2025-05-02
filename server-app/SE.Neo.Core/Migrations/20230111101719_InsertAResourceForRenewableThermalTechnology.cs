using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InsertAResourceForRenewableThermalTechnology : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[Resource] " +
                                 "([Content_Title] ," +
                                 "[Reference_Url], " +
                                 "[Type_Id], " +
                                 "[Created_Ts], " +
                                 "[Last_Change_Ts] , " +
                                 "[Article_Id], " +
                                 "[Tool_Id])" +
                                 "VALUES( " +
                                 "'What is Renewable Thermal Technology?'," +
                                 " ''," +
                                 "(SELECT Resource_Type_Id from [dbo].[Resource_Type] where Resource_Type_Name = 'PDF'), " +
                                 "GETDATE(), " +
                                 "GETDATE(), " +
                                 "(SELECT CMS_Article_Id from [dbo].[CMS_Article] where Article_Slug = 'what-is-renewable-thermal-technology'), " +
                                 "NULL)"
                                 );

            migrationBuilder.Sql("INSERT INTO [dbo].[Resource_Technology]  " +
                                "([Resource_Id] " +
                                ",[Technology_Id] " +
                                ",[Created_Ts] " +
                                ",[Last_Change_Ts]) " +
                                "VALUES( " +
                                "(SELECT [RESOURCE_ID] FROM [dbo].[Resource] WHERE [Content_Title] = 'What is Renewable Thermal Technology?'), " +
                                "(SELECT[CMS_Technology_Id] FROM[dbo].[CMS_Technology] WHERE[Technology_Slug] = 'renewable_thermal'), " +
                                "GETDATE() ," +
                                "GETDATE()) ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE from [dbo].[Resource_Technology] " +
                                 "WHERE Resource_Id in " +
                                 "(SELECT [RESOURCE_ID] FROM [dbo].[Resource] WHERE [Content_Title] = 'What is Renewable Thermal Technology?') " +
                                 "AND Technology_Id in " +
                                 "(SELECT[CMS_Technology_Id] FROM[dbo].[CMS_Technology] WHERE[Technology_Slug] = 'renewable_thermal')");


            migrationBuilder.DeleteData(
                 table: "Resource",
                 keyColumn: "Content_Title",
                 keyValue: "What is Renewable Thermal Technology?"
                );
        }
    }
}
