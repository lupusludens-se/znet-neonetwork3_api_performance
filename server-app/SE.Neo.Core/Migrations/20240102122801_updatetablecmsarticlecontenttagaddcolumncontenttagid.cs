using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updatetablecmsarticlecontenttagaddcolumncontenttagid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CMS_Content_Tag_Id",
                table: "CMS_Article_Content_Tag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Content_Tag_CMS_Content_Tag_Id",
                table: "CMS_Article_Content_Tag",
                column: "CMS_Content_Tag_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CMS_Article_Content_Tag_CMS_Content_Tag_CMS_Content_Tag_Id",
                table: "CMS_Article_Content_Tag",
                column: "CMS_Content_Tag_Id",
                principalTable: "CMS_Content_Tag",
                principalColumn: "CMS_Content_Tag_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMS_Article_Content_Tag_CMS_Content_Tag_CMS_Content_Tag_Id",
                table: "CMS_Article_Content_Tag");

            migrationBuilder.DropIndex(
                name: "IX_CMS_Article_Content_Tag_CMS_Content_Tag_Id",
                table: "CMS_Article_Content_Tag");

            migrationBuilder.DropColumn(
                name: "CMS_Content_Tag_Id",
                table: "CMS_Article_Content_Tag");
        }
    }
}
