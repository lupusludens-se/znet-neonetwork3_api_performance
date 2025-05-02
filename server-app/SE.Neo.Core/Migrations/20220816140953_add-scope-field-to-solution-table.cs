using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addscopefieldtosolutiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Scope",
                table: "CMS_Solution",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql("UPDATE[dbo].[CMS_Solution] SET[Scope] = 'Scope 2' where[Solution_Slug] = 'distributed-resources' " +
                "UPDATE[dbo].[CMS_Solution] SET[Scope] = 'Scope 1, 2' where[Solution_Slug] = 'eacs-and-offsets' " +
                "UPDATE[dbo].[CMS_Solution] SET[Scope] = 'Scope 2' where[Solution_Slug] = 'efficiency' " +
                "UPDATE[dbo].[CMS_Solution] SET[Scope] = 'Scope 2' where[Solution_Slug] = 'green-tariffs-and-renewable-energy' " +
                "UPDATE[dbo].[CMS_Solution] SET[Scope] = 'Scope 2' where[Solution_Slug] = 'large-scale-renewable-energy' " +
                "UPDATE[dbo].[CMS_Solution] SET[Scope] = 'Scope 3' where[Solution_Slug] = 'value-and-supply-chain'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scope",
                table: "CMS_Solution");
        }
    }
}
