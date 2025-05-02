using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class add_emerging_technology_relation_again_in_cms_category_technology : Migration
    {
        //The changes is laready merged to master branch and other branch as part of the development and deployment process. 
        //Commenting this code so that the Up and Down method doesnot get executed to avoid duplicate insertion to the table.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("INSERT INTO [dbo].[CMS_Category_Technology] " +
            //                  "([CMS_Category_Id],[CMS_Technology_Id]) " +
            //                  "Select CMS_Category_Id, tech.CMS_Technology_Id from[CMS_Category] " +
            //                  "CROSS JOIN(SELECT CMS_Technology_Id from[dbo].[CMS_Technology] where[Technology_Name] in ('Emerging Technology')) as tech " +
            //                  "WHERE [Category_Name] = 'Emerging Technologies'");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("DELETE from [dbo].[CMS_Category_Technology] " +
            //                     "WHERE CMS_Category_Id in " +
            //                     "(SELECT CMS_Category_Id from [dbo].[CMS_Category] where Category_Name = 'Emerging Technologies') " +
            //                     "AND CMS_Technology_Id in " +
            //                     "(SELECT CMS_Technology_Id from [dbo].[CMS_Technology] where Technology_Slug = 'emerging_technology')");
        }
    }
}
