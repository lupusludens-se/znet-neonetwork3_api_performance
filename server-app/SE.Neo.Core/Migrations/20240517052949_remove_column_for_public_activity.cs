using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class remove_column_for_public_activity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "Public_Site_Activity");

            migrationBuilder.DropColumn(
                name: "userAgent",
                table: "Public_Site_Activity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "Public_Site_Activity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "userAgent",
                table: "Public_Site_Activity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
