using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class sp_UpdateArticlePublicField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[sp_UpdateArticlePublicField]
                       @IDList NVARCHAR(MAX)
                       AS
                       BEGIN
                       DECLARE @SQLQuery NVARCHAR(MAX)
                       SET @SQLQuery = 'UPDATE CMS_Article SET Is_Public_Article = ~Is_Public_Article,
                                        MODIFIED_DATE = GETDATE()  WHERE CMS_Article_Id IN (' + @IDList + ')'
                                        EXEC sp_executesql @SQLQuery
                                        END";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[sp_UpdateArticlePublicField]";
            migrationBuilder.Sql(sp);

        }
    }
}
