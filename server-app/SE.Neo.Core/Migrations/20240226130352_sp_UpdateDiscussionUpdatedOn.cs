using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class sp_UpdateDiscussionUpdatedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[sp_UpdateDiscussionUpdatedOn]
                    @Discussion_Id int, @Discussion_UpdatedOn datetime2 
                AS
                BEGIN
                    SET NOCOUNT ON;
                    UPDATE [dbo].[Discussion] SET Discussion_Updated_Ts = @Discussion_UpdatedOn WHERE Discussion_Id = @Discussion_Id
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[sp_UpdateDiscussionUpdatedOn]";

            migrationBuilder.Sql(sp);

        }
    }
}
