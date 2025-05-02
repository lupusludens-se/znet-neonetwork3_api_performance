using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class FixedValueProvidedTypeMitigatingClimateChangeTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 8,
                column: "Value_Provided_Name",
                value: "Visible commitment to climate action");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 8,
                column: "Value_Provided_Name",
                value: "Visible comittment to climate action");
        }
    }
}
