using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class RemovedProjectOffsitePPADetailsIsExistingColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_Existing",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_Existing",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "bit",
                nullable: true);
        }
    }
}
