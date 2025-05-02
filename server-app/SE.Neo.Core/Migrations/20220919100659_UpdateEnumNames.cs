using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateEnumNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 8,
                column: "Contract_Structure_Name",
                value: "As-a-Service or Alternative Financing");

            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 8,
                column: "Value_Provided_Name",
                value: "Visible Commitment to Climate Action");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contract_Structure",
                keyColumn: "Contract_Structure_Id",
                keyValue: 8,
                column: "Contract_Structure_Name",
                value: "As-a-service or Alternative Financing");

            migrationBuilder.UpdateData(
                table: "Value_Provided",
                keyColumn: "Value_Provided_Id",
                keyValue: 8,
                column: "Value_Provided_Name",
                value: "Visible commitment to climate action");
        }
    }
}
