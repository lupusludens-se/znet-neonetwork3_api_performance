using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateSPAdminToolTip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Role] SET [ToolTip] = 'Permits heightened access to manage their company profile and associated users. Restricted to one SP Admin per company.' WHERE  [Role_Name]= 'SP Admin'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Role] SET [ToolTip] = '' WHERE  [Role_Name]= 'SP Admin'");

        }
    }
}
