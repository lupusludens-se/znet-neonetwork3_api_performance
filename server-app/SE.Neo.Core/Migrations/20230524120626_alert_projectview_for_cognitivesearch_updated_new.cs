using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class alert_projectview_for_cognitivesearch_updated_new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER VIEW [dbo].[ProjectSearchView] AS SELECT CONCAT('3_',Project_Id) as Id,Project_Id as Original_Id,Title as Subject, B.Status_Id as UserStatus, dbo.Project.Created_Ts, ISNULL(dbo.Project.Last_Change_Ts, dbo.Project.Created_Ts) as Last_Change_Ts, \r\nCAST(CASE WHEN dbo.Project.Status_Id > 2 THEN 1 ELSE 0 END AS bit) as Is_Deleted,Description, (SELECT TOP 1 dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name \r\nFROM dbo.Project p INNER JOIN dbo.CMS_Category ON p.Category_Id = dbo.CMS_Category.CMS_Category_Id WHERE p.Project_Id = dbo.Project.Project_Id FOR JSON AUTO) as Categories,'[]' as Solutions,\r\n(SELECT dbo.CMS_Technology.CMS_Technology_Id as TechnologyId, dbo.CMS_Technology.Technology_Name as Name FROM dbo.Project_Technology INNER JOIN dbo.CMS_Technology ON \r\ndbo.Project_Technology.Technology_Id = dbo.CMS_Technology.CMS_Technology_Id WHERE dbo.Project_Technology.Project_Id = dbo.Project.Project_Id FOR JSON AUTO) as Technologies,\r\n(SELECT dbo.CMS_Region.CMS_Region_Id as RegionId, dbo.CMS_Region.Region_Name as Name FROM dbo.Project_Region INNER JOIN dbo.CMS_Region ON \r\ndbo.Project_Region.Region_Id = dbo.CMS_Region.CMS_Region_Id WHERE dbo.Project_Region.Project_Id = dbo.Project.Project_Id FOR JSON AUTO) as Regions,'[]' as Allowed_Regions,\r\n'[]' as Allowed_Roles,'[]' as Allowed_Categories,'[]' as Allowed_Users,\r\n'[]' as Allowed_Companies,CAST(0 AS bit) as Is_Private,3 as Entity_Type FROM dbo.Project\r\nINNER JOIN [User] B on B.User_Id = dbo.Project.Created_User_Id");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER VIEW [dbo].[ProjectSearchView] AS SELECT CONCAT('3_',Project_Id) as Id,Project_Id as Original_Id,Title as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts, \r\nCAST(CASE WHEN Status_Id > 2 THEN 1 ELSE 0 END AS bit) as Is_Deleted,Description, (SELECT TOP 1 dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name \r\nFROM dbo.Project p INNER JOIN dbo.CMS_Category ON p.Category_Id = dbo.CMS_Category.CMS_Category_Id WHERE p.Project_Id = dbo.Project.Project_Id FOR JSON AUTO) as Categories,'[]' as Solutions,\r\n(SELECT dbo.CMS_Technology.CMS_Technology_Id as TechnologyId, dbo.CMS_Technology.Technology_Name as Name FROM dbo.Project_Technology INNER JOIN dbo.CMS_Technology ON \r\ndbo.Project_Technology.Technology_Id = dbo.CMS_Technology.CMS_Technology_Id WHERE dbo.Project_Technology.Project_Id = dbo.Project.Project_Id FOR JSON AUTO) as Technologies,\r\n(SELECT dbo.CMS_Region.CMS_Region_Id as RegionId, dbo.CMS_Region.Region_Name as Name FROM dbo.Project_Region INNER JOIN dbo.CMS_Region ON \r\ndbo.Project_Region.Region_Id = dbo.CMS_Region.CMS_Region_Id WHERE dbo.Project_Region.Project_Id = dbo.Project.Project_Id FOR JSON AUTO) as Regions,'[]' as Allowed_Regions,\r\n'[]' as Allowed_Roles,'[]' as Allowed_Categories,'[]' as Allowed_Users,\r\n'[]' as Allowed_Companies,CAST(0 AS bit) as Is_Private,3 as Entity_Type FROM dbo.Project");
        }
    }
}
