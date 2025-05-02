using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class Add_Is_Deleted_Column_for_CMS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "CMS_Technology",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "CMS_Solution",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "CMS_Region",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "CMS_Category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "CMS_Technology");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "CMS_Solution");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "CMS_Region");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "CMS_Category");
        }
    }
}
