using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class TooltipForRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE [dbo].[Role] SET [ToolTip] = 'Permits grant access to manage their own company profile and their own users." +
                " Only one SP Admin is allowed for a company.' WHERE  [Role_Name]= 'SP Admin'");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Role] SET [ToolTip] = NULL WHERE  [Role_Name]= 'SP Admin'");
        }
    }
}
