using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class add_description_field_to_taxonomies_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Regiony_Slug",
                table: "CMS_Region",
                newName: "Region_Slug");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CMS_Technology",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CMS_Solution",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CMS_Region",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CMS_Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE [dbo].[CMS_Category] SET [Category_Name] = 'Deleted' " +
    "UPDATE[dbo].[CMS_Technology] SET[Technology_Name] = 'Deleted' " +
    "UPDATE[dbo].[CMS_Solution] SET[Solution_Name] = 'Deleted' " +
    "UPDATE[dbo].[CMS_Region] SET[Region_Name] = 'Deleted'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CMS_Technology");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CMS_Solution");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CMS_Region");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CMS_Category");

            migrationBuilder.RenameColumn(
                name: "Region_Slug",
                table: "CMS_Region",
                newName: "Regiony_Slug");
        }
    }
}
