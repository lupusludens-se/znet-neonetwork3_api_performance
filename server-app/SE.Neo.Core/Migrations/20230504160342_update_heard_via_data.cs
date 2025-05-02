using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class update_heard_via_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Heard_Via] SET Heard_Via_Name= 'Zeigo Network Member' WHERE [Heard_Via_Name] = 'NEO Network Member'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Heard_Via] SET Heard_Via_Name= 'Zeigo Network Member' WHERE [Heard_Via_Name] = 'Zeigo Network Member'");
        }
    }
}
