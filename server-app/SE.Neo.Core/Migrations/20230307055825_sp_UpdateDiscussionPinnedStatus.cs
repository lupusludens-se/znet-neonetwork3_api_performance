using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class sp_UpdateDiscussionPinnedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[sp_UpdateDiscussionPinnedStatus]
                    @Discussion_Id int, @IsPinned bit 
                AS
                BEGIN
                    SET NOCOUNT ON;
                    UPDATE [dbo].[Discussion] SET Discussion_Is_Pinned = @IsPinned WHERE Discussion_Id = @Discussion_Id
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[sp_UpdateDiscussionPinnedStatus]";

            migrationBuilder.Sql(sp);
        }
    }
}
