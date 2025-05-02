using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class update_project_redource_add_article_tool_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Article_Id",
                table: "Resource",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tool_Id",
                table: "Resource",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Article_Id",
                table: "Resource",
                column: "Article_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Tool_Id",
                table: "Resource",
                column: "Tool_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_CMS_Article_Article_Id",
                table: "Resource",
                column: "Article_Id",
                principalTable: "CMS_Article",
                principalColumn: "CMS_Article_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Tool_Tool_Id",
                table: "Resource",
                column: "Tool_Id",
                principalTable: "Tool",
                principalColumn: "Tool_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_CMS_Article_Article_Id",
                table: "Resource");

            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Tool_Tool_Id",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_Article_Id",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_Tool_Id",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "Article_Id",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "Tool_Id",
                table: "Resource");
        }
    }
}
