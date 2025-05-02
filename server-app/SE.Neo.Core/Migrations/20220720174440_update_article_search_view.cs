using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class update_article_search_view : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMS_Article_Role_Role_CMS_Role_Id",
                table: "CMS_Article_Role");

            migrationBuilder.RenameColumn(
                name: "CMS_Role_Id",
                table: "CMS_Article_Role",
                newName: "Role_Id");

            migrationBuilder.RenameIndex(
                name: "IX_CMS_Article_Role_CMS_Role_Id",
                table: "CMS_Article_Role",
                newName: "IX_CMS_Article_Role_Role_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CMS_Article_Role_Role_Role_Id",
                table: "CMS_Article_Role",
                column: "Role_Id",
                principalTable: "Role",
                principalColumn: "Role_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("DROP VIEW [dbo].[ArticleSearchView]");

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
                "ISNULL((SELECT CONVERT(varchar, dbo.CMS_Article_Role.Role_Id) as AllowedRoleId " +
                "FROM dbo.CMS_Article_Role WHERE dbo.CMS_Article_Role.CMS_Article_Id = dbo.CMS_Article.CMS_Article_Id FOR JSON AUTO),'[]') as Allowed_Roles, " +
                " '[]' as Allowed_Categories, '[]' as Allowed_Regions, '[]' as Allowed_Companies,'[]' as Allowed_Users," +
                "CAST(0 AS bit) as Is_Private," +
                "2 as Entity_Type " +
                "FROM dbo.CMS_Article");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMS_Article_Role_Role_Role_Id",
                table: "CMS_Article_Role");

            migrationBuilder.RenameColumn(
                name: "Role_Id",
                table: "CMS_Article_Role",
                newName: "CMS_Role_Id");

            migrationBuilder.RenameIndex(
                name: "IX_CMS_Article_Role_Role_Id",
                table: "CMS_Article_Role",
                newName: "IX_CMS_Article_Role_CMS_Role_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CMS_Article_Role_Role_CMS_Role_Id",
                table: "CMS_Article_Role",
                column: "CMS_Role_Id",
                principalTable: "Role",
                principalColumn: "Role_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("DROP VIEW [dbo].[ArticleSearchView]");

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
        }
    }
}