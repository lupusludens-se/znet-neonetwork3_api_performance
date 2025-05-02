using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updatesummaryemailsettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[User_Email_Alert] SET Frequency = 2 WHERE Frequency = 1 AND Email_Alert_Id = 3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
