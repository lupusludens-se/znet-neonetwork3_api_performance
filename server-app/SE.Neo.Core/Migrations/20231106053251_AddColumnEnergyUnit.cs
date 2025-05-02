using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddColumnEnergyUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Minimum_Annual_KWh",
                table: "Project_Emerging_Technology_Details",
                newName: "Minimum_Annual_Value");

            migrationBuilder.AddColumn<int>(
                name: "Energy_Unit_Id",
                table: "Project_Emerging_Technology_Details",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Energy_Unit_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.RenameColumn(
                name: "Minimum_Annual_Value",
                table: "Project_Emerging_Technology_Details",
                newName: "Minimum_Annual_KWh");
        }
    }
}
