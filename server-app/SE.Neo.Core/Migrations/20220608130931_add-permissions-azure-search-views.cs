using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addpermissionsazuresearchviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[ToolSearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[EventSearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[CompanySearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[ProjectSearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[ArticleSearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[ForumSearchView]");

            migrationBuilder.Sql(
                "CREATE VIEW [dbo].[ForumSearchView] AS " +
                "SELECT CONCAT('1_',Discussion_Id) as Id,Discussion_Id as Original_Id," +
                "Discussion_Subject as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts, Discussion_Is_Deleted as Is_Deleted," +
                "CAST(CASE WHEN dbo.Discussion.Discussion_Type = 2 THEN 1 ELSE 0 END AS bit) as Is_Private," +
                "(SELECT TOP 1 Message_Text " +
                "FROM dbo.Message " +
                "WHERE dbo.Message.Parent_Message_Id IS NULL AND dbo.Message.Discussion_Id = dbo.Discussion.Discussion_Id) as Description," +
                "(SELECT dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
                "FROM dbo.Discussion_Category " +
                "INNER JOIN dbo.CMS_Category ON dbo.Discussion_Category.Category_Id = dbo.CMS_Category.CMS_Category_Id " +
                "WHERE dbo.Discussion_Category.Disscussion_Id = dbo.Discussion.Discussion_Id " +
                "FOR JSON AUTO) as Categories," +
                "'[]' as Solutions," +
                "'[]' as Technologies," +
                "(SELECT dbo.CMS_Region.CMS_Region_Id as RegionId, dbo.CMS_Region.Region_Name as Name " +
                "FROM dbo.Discussion_Region " +
                "INNER JOIN dbo.CMS_Region ON dbo.Discussion_Region.Region_Id = dbo.CMS_Region.CMS_Region_Id " +
                "WHERE dbo.Discussion_Region.Disscussion_Id = dbo.Discussion.Discussion_Id " +
                "FOR JSON AUTO) as Regions," +
                "(SELECT dbo.Discussion_User.User_Id as AllowedUserId " +
                "FROM dbo.Discussion_User " +
                "WHERE dbo.Discussion_User.Discussion_Id = dbo.Discussion.Discussion_Id " +
                "FOR JSON AUTO) as Allowed_Users, " +
                "'[]' as Allowed_Roles, '[]' as Allowed_Categories, '[]' as Allowed_Regions, '[]' as Allowed_Companies," +
                "1 as Entity_Type " +
                "FROM dbo.Discussion " +
                "WHERE (dbo.Discussion.Discussion_Type = 2 OR dbo.Discussion.Discussion_Type = 3)");

            migrationBuilder.Sql(
                "CREATE VIEW [dbo].[ArticleSearchView] AS " +
                "SELECT CONCAT('2_',CMS_Article_Id) as Id,CMS_Article_Id as Original_Id," +
                "Title as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts, Is_Deleted, Content as Description," +
                "(SELECT dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
                "FROM dbo.CMS_Article_Category " +
                "INNER JOIN dbo.CMS_Category ON dbo.CMS_Article_Category.CMS_Category_Id = dbo.CMS_Category.CMS_Category_Id " +
                "WHERE dbo.CMS_Article_Category.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id " +
                "FOR JSON AUTO) as Categories," +
                "(SELECT dbo.CMS_Solution.CMS_Solution_Id as SolutionId, dbo.CMS_Solution.Solution_Name as Name " +
                "FROM dbo.CMS_Article_Solution " +
                "INNER JOIN dbo.CMS_Solution ON dbo.CMS_Article_Solution.CMS_Solution_Id = dbo.CMS_Solution.CMS_Solution_Id " +
                "WHERE dbo.CMS_Article_Solution.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id " +
                "FOR JSON AUTO) as Solutions," +
                "(SELECT dbo.CMS_Technology.CMS_Technology_Id as TechnologyId, dbo.CMS_Technology.Technology_Name as Name " +
                "FROM dbo.CMS_Article_Technology " +
                "INNER JOIN dbo.CMS_Technology ON dbo.CMS_Article_Technology.CMS_Technology_Id = dbo.CMS_Technology.CMS_Technology_Id " +
                "WHERE dbo.CMS_Article_Technology.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id " +
                "FOR JSON AUTO) as Technologies," +
                "(SELECT dbo.CMS_Region.CMS_Region_Id as RegionId, dbo.CMS_Region.Region_Name as Name " +
                "FROM dbo.CMS_Article_Region " +
                "INNER JOIN dbo.CMS_Region ON dbo.CMS_Article_Region.CMS_Region_Id = dbo.CMS_Region.CMS_Region_Id " +
                "WHERE dbo.CMS_Article_Region.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id " +
                "FOR JSON AUTO) as Regions," +
                "'[]' as Allowed_Roles, '[]' as Allowed_Categories, '[]' as Allowed_Regions, '[]' as Allowed_Companies,'[]' as Allowed_Users," +
                "CAST(0 AS bit) as Is_Private," +
                "2 as Entity_Type " +
                "FROM dbo.CMS_Article");

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
               "(SELECT dbo.Project.Company_Id as AllowedCompanyId " +
               "FROM dbo.Project " +
               "FOR JSON AUTO) as Allowed_Companies," +
               "CAST(0 AS bit) as Is_Private," +
               "3 as Entity_Type " +
               "FROM dbo.Project");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[CompanySearchView] AS " +
               "SELECT CONCAT('4_',Company_Id) as Id,Company_Id as Original_Id," +
               "Company_Name as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts," +
               "CAST(CASE WHEN Status_Id = 3 THEN 1 ELSE 0 END AS bit) as Is_Deleted,About as Description," +
               "(SELECT dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
               "FROM dbo.Company_Category " +
               "INNER JOIN dbo.CMS_Category ON dbo.Company_Category.Category_Id = dbo.CMS_Category.CMS_Category_Id " +
               "WHERE dbo.Company_Category.Company_Id = dbo.Company.Company_Id " +
               "FOR JSON AUTO) as Categories," +
               "'[]' as Solutions," +
               "'[]' as Technologies," +
               "'[]' as Regions," +
               "'[]' as Allowed_Roles, '[]' as Allowed_Categories, '[]' as Allowed_Regions, '[]' as Allowed_Companies,'[]' as Allowed_Users," +
               "CAST(0 AS bit) as Is_Private," +
               "4 as Entity_Type " +
               "FROM dbo.Company");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[EventSearchView] AS " +
               "SELECT CONCAT('5_',Event_Id) as Id,Event_Id as Original_Id," +
               "Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts," +
               "CAST(0 as bit) as Is_Deleted, Description," +
               "(SELECT dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
               "FROM dbo.Event_Category " +
               "INNER JOIN dbo.CMS_Category ON dbo.Event_Category.Category_Id = dbo.CMS_Category.CMS_Category_Id " +
               "WHERE dbo.Event_Category.Event_Id = dbo.Event.Event_Id " +
               "FOR JSON AUTO) as Categories," +
               "(SELECT dbo.Event_Invited_Category.Category_Id as AllowedCategoryId " +
               "FROM dbo.Event_Invited_Category " +
               "WHERE dbo.Event_Invited_Category.Event_Id = dbo.Event.Event_Id " +
               "FOR JSON AUTO) as Allowed_Categories," +
               "(SELECT dbo.Event_Invited_Role.Role_Id as AllowedRoleId " +
               "FROM dbo.Event_Invited_Role " +
               "WHERE dbo.Event_Invited_Role.Event_Id = dbo.Event.Event_Id " +
               "FOR JSON AUTO) as Allowed_Roles," +
               "(SELECT dbo.Event_Invited_Region.Region_Id as AllowedRegionId " +
               "FROM dbo.Event_Invited_Region " +
               "WHERE dbo.Event_Invited_Region.Event_Id = dbo.Event.Event_Id " +
               "FOR JSON AUTO) as Allowed_Regions," +
               "(SELECT dbo.Event_Invited_User.User_Id as AllowedUserId " +
               "FROM dbo.Event_Invited_User " +
               "Where dbo.Event_Invited_User.Event_Id = dbo.Event.Event_Id " +
               "FOR JSON AUTO) AS Allowed_Users," +
               "'[]' as Solutions,'[]' as Technologies,'[]' as Regions, '[]' as Allowed_Companies," +
               "CAST(0 AS bit) as Is_Private," +
               "5 as Entity_Type " +
               "FROM dbo.Event");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[ToolSearchView] AS " +
               "SELECT CONCAT('6_',Tool_Id) as Id,Tool_Id as Original_Id," +
               "Title as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts," +
               "CAST(1 ^ Is_Active as BIT) AS Is_Deleted, Description," +
               "(SELECT dbo.Tool_Role.Role_Id as AllowedRoleId " +
               "FROM dbo.Tool_Role " +
               "WHERE dbo.Tool_Role.Tool_Id = dbo.Tool.Tool_Id " +
               "FOR JSON AUTO) as Allowed_Roles," +
               "(SELECT dbo.Tool_Company.Company_Id as AllowedCompanyId " +
               "FROM dbo.Tool_Company " +
               "WHERE dbo.Tool_Company.Tool_Id = dbo.Tool.Tool_Id " +
               "FOR JSON AUTO) as Allowed_Companies," +
               "'[]' as Categories,'[]' as Solutions,'[]' as Technologies,'[]' as Regions," +
               "'[]' as Allowed_Categories,'[]' as Allowed_Regions, '[]' as Allowed_Users," +
               "CAST(0 AS bit) as Is_Private," +
               "6 as Entity_Type " +
               "FROM dbo.Tool");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[ToolSearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[EventSearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[CompanySearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[ProjectSearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[ArticleSearchView]");
            migrationBuilder.Sql("DROP VIEW [dbo].[ForumSearchView]");
            migrationBuilder.Sql(
                "CREATE VIEW [dbo].[ForumSearchView] AS " +
                "SELECT CONCAT('1_',Discussion_Id) as Id,Discussion_Id as Original_Id," +
                "Discussion_Subject as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts, Discussion_Is_Deleted as Is_Deleted," +
                "(SELECT TOP 1 Message_Text " +
                "FROM dbo.Message " +
                "WHERE dbo.Message.Parent_Message_Id IS NULL AND dbo.Message.Discussion_Id = dbo.Discussion.Discussion_Id) as Description," +
                "(SELECT dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
                "FROM dbo.Discussion_Category " +
                "INNER JOIN dbo.CMS_Category ON dbo.Discussion_Category.Category_Id = dbo.CMS_Category.CMS_Category_Id " +
                "WHERE dbo.Discussion_Category.Disscussion_Id = dbo.Discussion.Discussion_Id " +
                "FOR JSON AUTO) as Categories," +
                "'[]' as Solutions," +
                "'[]' as Technologies," +
                "(SELECT dbo.CMS_Region.CMS_Region_Id as RegionId, dbo.CMS_Region.Region_Name as Name " +
                "FROM dbo.Discussion_Region " +
                "INNER JOIN dbo.CMS_Region ON dbo.Discussion_Region.Region_Id = dbo.CMS_Region.CMS_Region_Id " +
                "WHERE dbo.Discussion_Region.Disscussion_Id = dbo.Discussion.Discussion_Id " +
                "FOR JSON AUTO) as Regions," +
                "1 as Entity_Type " +
                "FROM dbo.Discussion " +
                "WHERE (dbo.Discussion.Discussion_Type = 2)");

            migrationBuilder.Sql(
                "CREATE VIEW [dbo].[ArticleSearchView] AS " +
                "SELECT CONCAT('2_',CMS_Article_Id) as Id,CMS_Article_Id as Original_Id," +
                "Title as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts, Is_Deleted, Content as Description," +
                "(SELECT dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
                "FROM dbo.CMS_Article_Category " +
                "INNER JOIN dbo.CMS_Category ON dbo.CMS_Article_Category.CMS_Category_Id = dbo.CMS_Category.CMS_Category_Id " +
                "WHERE dbo.CMS_Article_Category.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id " +
                "FOR JSON AUTO) as Categories," +
                "(SELECT dbo.CMS_Solution.CMS_Solution_Id as SolutionId, dbo.CMS_Solution.Solution_Name as Name " +
                "FROM dbo.CMS_Article_Solution " +
                "INNER JOIN dbo.CMS_Solution ON dbo.CMS_Article_Solution.CMS_Solution_Id = dbo.CMS_Solution.CMS_Solution_Id " +
                "WHERE dbo.CMS_Article_Solution.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id " +
                "FOR JSON AUTO) as Solutions," +
                "(SELECT dbo.CMS_Technology.CMS_Technology_Id as TechnologyId, dbo.CMS_Technology.Technology_Name as Name " +
                "FROM dbo.CMS_Article_Technology " +
                "INNER JOIN dbo.CMS_Technology ON dbo.CMS_Article_Technology.CMS_Technology_Id = dbo.CMS_Technology.CMS_Technology_Id " +
                "WHERE dbo.CMS_Article_Technology.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id " +
                "FOR JSON AUTO) as Technologies," +
                "(SELECT dbo.CMS_Region.CMS_Region_Id as RegionId, dbo.CMS_Region.Region_Name as Name " +
                "FROM dbo.CMS_Article_Region " +
                "INNER JOIN dbo.CMS_Region ON dbo.CMS_Article_Region.CMS_Region_Id = dbo.CMS_Region.CMS_Region_Id " +
                "WHERE dbo.CMS_Article_Region.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id " +
                "FOR JSON AUTO) as Regions," +
                "2 as Entity_Type " +
                "FROM dbo.CMS_Article");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[ProjectSearchView] AS " +
               "SELECT CONCAT('3_',Project_Id) as Id,Project_Id as Original_Id," +
               "Title as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts, " +
               "CAST(CASE WHEN Status_Id = 4 THEN 1 ELSE 0 END AS bit) as Is_Deleted,Description," +
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
               "3 as Entity_Type " +
               "FROM dbo.Project");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[CompanySearchView] AS " +
               "SELECT CONCAT('4_',Company_Id) as Id,Company_Id as Original_Id," +
               "Company_Name as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts," +
               "CAST(CASE WHEN Status_Id = 3 THEN 1 ELSE 0 END AS bit) as Is_Deleted,About as Description," +
               "(SELECT dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
               "FROM dbo.Company_Category " +
               "INNER JOIN dbo.CMS_Category ON dbo.Company_Category.Category_Id = dbo.CMS_Category.CMS_Category_Id " +
               "WHERE dbo.Company_Category.Company_Id = dbo.Company.Company_Id " +
               "FOR JSON AUTO) as Categories," +
               "'[]' as Solutions," +
               "'[]' as Technologies," +
               "'[]' as Regions," +
               "4 as Entity_Type " +
               "FROM dbo.Company");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[EventSearchView] AS " +
               "SELECT CONCAT('5_',Event_Id) as Id,Event_Id as Original_Id," +
               "Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts," +
               "CAST(0 as bit) as Is_Deleted, Description," +
               "(SELECT dbo.CMS_Category.CMS_Category_Id as CategoryId, dbo.CMS_Category.Category_Name as Name " +
               "FROM dbo.Event_Category " +
               "INNER JOIN dbo.CMS_Category ON dbo.Event_Category.Category_Id = dbo.CMS_Category.CMS_Category_Id " +
               "WHERE dbo.Event_Category.Event_Id = dbo.Event.Event_Id " +
               "FOR JSON AUTO) as Categories," +
               "'[]' as Solutions,'[]' as Technologies,'[]' as Regions," +
               "5 as Entity_Type " +
               "FROM dbo.Event");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[ToolSearchView] AS " +
               "SELECT CONCAT('6_',Tool_Id) as Id,Tool_Id as Original_Id," +
               "Title as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts," +
               "(1 ^ Is_Active) as Is_Deleted, Description," +
               "'[]' as Categories,'[]' as Solutions,'[]' as Technologies,'[]' as Regions," +
               "6 as Entity_Type " +
               "FROM dbo.Tool");
        }
    }
}
