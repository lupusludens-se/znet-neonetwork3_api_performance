using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updatecompanysearchview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[CompanySearchView]");

            migrationBuilder.Sql(
               "CREATE VIEW [dbo].[CompanySearchView] AS " +
               "SELECT CONCAT('4_',Company_Id) as Id,Company_Id as Original_Id," +
               "Company_Name as Subject, Created_Ts, ISNULL(Last_Change_Ts, Created_Ts) as Last_Change_Ts," +
               "CAST(CASE WHEN Status_Id > 1 THEN 1 ELSE 0 END AS bit) as Is_Deleted,About as Description," +
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[CompanySearchView]");

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
        }
    }
}
