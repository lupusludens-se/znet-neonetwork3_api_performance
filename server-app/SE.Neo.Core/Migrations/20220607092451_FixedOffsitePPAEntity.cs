using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class FixedOffsitePPAEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Additional_Comments",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropColumn(
                name: "Time_And_Urgency_Considerations",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Additional_Comments",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Time_And_Urgency_Considerations",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
