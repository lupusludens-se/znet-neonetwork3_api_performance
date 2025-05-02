using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class RemovedCustomFieldsForOffsitePPADetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Custom_EAC",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropColumn(
                name: "Custom_ISORTO",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropColumn(
                name: "Custom_Settlement_Hub_Or_Load_Zone",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Custom_EAC",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Custom_ISORTO",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Custom_Settlement_Hub_Or_Load_Zone",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
