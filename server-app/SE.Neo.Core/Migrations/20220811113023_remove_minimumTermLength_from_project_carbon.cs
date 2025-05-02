using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class remove_minimumTermLength_from_project_carbon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Minimum_Term_Length",
                table: "Project_Carbon_Offsets_Details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Minimum_Term_Length",
                table: "Project_Carbon_Offsets_Details",
                type: "int",
                nullable: true);
        }
    }
}
