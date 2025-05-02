using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddNewColumnInArticleTableForPublicArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_Public_Article",
                table: "CMS_Article",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_Public_Article",
                table: "CMS_Article");
        }
    }
}
