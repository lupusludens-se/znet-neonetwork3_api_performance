using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class InsertDataInCMSCategoryTechnology : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[CMS_Category_Technology] " +
                                "([CMS_Category_Id],[CMS_Technology_Id]) " +
                                "Select CMS_Category_Id, tech.CMS_Technology_Id from[CMS_Category] " +
                                "CROSS JOIN(SELECT CMS_Technology_Id from[dbo].[CMS_Technology] where[Technology_Name] in ('Renewable Thermal')) as tech " +
                                "WHERE [Category_Name] = 'Emerging Technologies'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE from [dbo].[CMS_Category_Technology] " +
                                 "WHERE CMS_Category_Id in " +
                                 "(SELECT CMS_Category_Id from [dbo].[CMS_Category] where Category_Name = 'Emerging Technologies') " +
                                 "AND CMS_Technology_Id in " +
                                 "(SELECT CMS_Technology_Id from [dbo].[CMS_Technology] where Technology_Slug = 'renewable_thermal')");
        }
    }
}
