using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updateprojectsearchview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[ProjectSearchView]");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[ProjectSearchView] AS " +
               "SELECT CONCAT('3_',Project_Id) as Id,Project_Id as Original_Id," +
               "Title as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts, " +
               "CAST(CASE WHEN Status_Id > 2 THEN 1 ELSE 0 END AS bit) as Is_Deleted,Description, " +
               "(SELECT TOP 1 dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
               "FROM dbo.Project p " +
               "INNER JOIN dbo.CMS_Category ON p.Category_Id = dbo.CMS_Category.CMS_Category_Id " +
               "WHERE p.Project_Id = dbo.Project.Project_Id " +
               "FOR JSON AUTO) as Categories," +
               "'[]' as Solutions," +
               "(SELECT dbo.CMS_Technology.CMS_Technology_Id as TechnologyId, dbo.CMS_Technology.Technology_Name as Name " +
               "FROM dbo.Project_Technology " +
               "INNER JOIN dbo.CMS_Technology ON dbo.Project_Technology.Technology_Id = dbo.CMS_Technology.CMS_Technology_Id " +
               "WHERE dbo.Project_Technology.Project_Id = dbo.Project.Project_Id " +
               "FOR JSON AUTO) as Technologies," +
               "(SELECT dbo.CMS_Region.CMS_Region_Id as RegionId, dbo.CMS_Region.Region_Name as Name " +
               "FROM dbo.Project_Region " +
               "INNER JOIN dbo.CMS_Region ON dbo.Project_Region.Region_Id = dbo.CMS_Region.CMS_Region_Id " +
               "WHERE dbo.Project_Region.Project_Id = dbo.Project.Project_Id " +
               "FOR JSON AUTO) as Regions," +
               "'[]' as Allowed_Regions,'[]' as Allowed_Roles,'[]' as Allowed_Categories,'[]' as Allowed_Users, '[]' as Allowed_Companies," +
               "CAST(0 AS bit) as Is_Private," +
               "3 as Entity_Type " +
               "FROM dbo.Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[ProjectSearchView]");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[ProjectSearchView] AS " +
               "SELECT CONCAT('3_',Project_Id) as Id,Project_Id as Original_Id," +
               "Title as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts, " +
               "CAST(CASE WHEN Status_Id > 2 THEN 1 ELSE 0 END AS bit) as Is_Deleted,Description, " +
               "(SELECT TOP 1 dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
               "FROM dbo.Project p " +
               "INNER JOIN dbo.CMS_Category ON p.Category_Id = dbo.CMS_Category.CMS_Category_Id " +
               "WHERE p.Project_Id = dbo.Project.Project_Id " +
               "FOR JSON AUTO) as Categories," +
               "'[]' as Solutions," +
               "(SELECT dbo.CMS_Technology.CMS_Technology_Id as TechnologyId, dbo.CMS_Technology.Technology_Name as Name " +
               "FROM dbo.Project_Technology " +
               "INNER JOIN dbo.CMS_Technology ON dbo.Project_Technology.Technology_Id = dbo.CMS_Technology.CMS_Technology_Id " +
               "WHERE dbo.Project_Technology.Project_Id = dbo.Project.Project_Id " +
               "FOR JSON AUTO) as Technologies," +
               "(SELECT dbo.CMS_Region.CMS_Region_Id as RegionId, dbo.CMS_Region.Region_Name as Name " +
               "FROM dbo.Project_Region " +
               "INNER JOIN dbo.CMS_Region ON dbo.Project_Region.Region_Id = dbo.CMS_Region.CMS_Region_Id " +
               "WHERE dbo.Project_Region.Project_Id = dbo.Project.Project_Id " +
               "FOR JSON AUTO) as Regions," +
               "'[]' as Allowed_Regions,'[]' as Allowed_Roles,'[]' as Allowed_Categories,'[]' as Allowed_Users," +
               "(SELECT CONVERT(varchar, dbo.Project.Company_Id) as AllowedCompanyId " +
               "FROM dbo.Project p " +
               "WHERE p.Project_Id = dbo.Project.Project_Id " +
               "FOR JSON AUTO) as Allowed_Companies," +
               "CAST(0 AS bit) as Is_Private," +
               "3 as Entity_Type " +
               "FROM dbo.Project");
        }
    }
}
