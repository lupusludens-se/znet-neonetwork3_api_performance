using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class update_project_TermLengthType_enum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Term_Length",
                keyColumn: "Term_Length_Id",
                keyValue: 1,
                column: "Term_Length_Name",
                value: "12 months");

            migrationBuilder.UpdateData(
                table: "Term_Length",
                keyColumn: "Term_Length_Id",
                keyValue: 2,
                column: "Term_Length_Name",
                value: "24 months");

            migrationBuilder.UpdateData(
                table: "Term_Length",
                keyColumn: "Term_Length_Id",
                keyValue: 3,
                column: "Term_Length_Name",
                value: "36 months");

            migrationBuilder.UpdateData(
                table: "Term_Length",
                keyColumn: "Term_Length_Id",
                keyValue: 4,
                column: "Term_Length_Name",
                value: "> 36 months");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Term_Length",
                keyColumn: "Term_Length_Id",
                keyValue: 1,
                column: "Term_Length_Name",
                value: "12 month");

            migrationBuilder.UpdateData(
                table: "Term_Length",
                keyColumn: "Term_Length_Id",
                keyValue: 2,
                column: "Term_Length_Name",
                value: "24 month");

            migrationBuilder.UpdateData(
                table: "Term_Length",
                keyColumn: "Term_Length_Id",
                keyValue: 3,
                column: "Term_Length_Name",
                value: "36 month");

            migrationBuilder.UpdateData(
                table: "Term_Length",
                keyColumn: "Term_Length_Id",
                keyValue: 4,
                column: "Term_Length_Name",
                value: "> 36 month");
        }
    }
}
