using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updateheard_viatypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 5,
                column: "Heard_Via_Name",
                value: "Referred by Schneider Electric Contact");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Heard_Via",
                keyColumn: "Heard_Via_Id",
                keyValue: 5,
                column: "Heard_Via_Name",
                value: "Referrend by Schneider Electric Contact");
        }
    }
}
