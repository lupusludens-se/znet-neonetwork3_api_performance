using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateValueInInitiativeScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Initiative_Scale] SET [Name] = 'Regional (Choose one or more continents)' WHERE  [Initiative_Scale_Id]= 3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Initiative_Scale] SET [Name] = 'Regional(Choose a continent)' WHERE  [Initiative_Scale_Id]= 3");
        }
    }
}
