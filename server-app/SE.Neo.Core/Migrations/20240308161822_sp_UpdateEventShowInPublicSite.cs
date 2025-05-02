using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class sp_UpdateEventShowInPublicSite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[sp_UpdateEventShowInPublicSite]
                    @Event_Id INT, @Show_In_Public_Site BIT 
                AS
                BEGIN                    
                    UPDATE [dbo].[Event] SET Show_In_Public_Site = @Show_In_Public_Site WHERE Event_Id = @Event_Id
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[sp_UpdateEventShowInPublicSite]";

            migrationBuilder.Sql(sp);
        }
    }
}
