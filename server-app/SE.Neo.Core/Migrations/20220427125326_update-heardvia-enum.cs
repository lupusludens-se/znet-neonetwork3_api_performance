using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updateheardviaenum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 15,
                column: "Heard_Via_Name",
                value: "I am an Employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 15,
                column: "Heard_Via_Name",
                value: "I am an employee");
        }
    }
}
