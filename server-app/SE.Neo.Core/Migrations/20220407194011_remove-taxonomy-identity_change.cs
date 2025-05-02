using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class removetaxonomyidentity_change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMS_Region_CMS_Region_Parent_Region_Id",
                table: "CMS_Region");

            migrationBuilder.DropIndex(
                name: "IX_CMS_Region_Parent_Region_Id",
                table: "CMS_Region");

            migrationBuilder.DropColumn(
                name: "Technology_CMS_Id",
                table: "CMS_Technology");

            migrationBuilder.DropColumn(
                name: "Solution_CMS_Id",
                table: "CMS_Solution");

            migrationBuilder.DropColumn(
                name: "Region_CMS_Id",
                table: "CMS_Region");

            migrationBuilder.DropColumn(
                name: "Category_CMS_Id",
                table: "CMS_Category");

            migrationBuilder.RenameColumn(
                name: "Parent_Region_Id",
                table: "CMS_Region",
                newName: "CMS_Parent_Region_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CMS_Parent_Region_Id",
                table: "CMS_Region",
                newName: "Parent_Region_Id");

            migrationBuilder.AddColumn<int>(
                name: "Technology_CMS_Id",
                table: "CMS_Technology",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Solution_CMS_Id",
                table: "CMS_Solution",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Region_CMS_Id",
                table: "CMS_Region",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Category_CMS_Id",
                table: "CMS_Category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Region_Parent_Region_Id",
                table: "CMS_Region",
                column: "Parent_Region_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CMS_Region_CMS_Region_Parent_Region_Id",
                table: "CMS_Region",
                column: "Parent_Region_Id",
                principalTable: "CMS_Region",
                principalColumn: "CMS_Region_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
