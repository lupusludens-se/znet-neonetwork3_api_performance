using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class modifycompanyentityaddindustryfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Industry_Id",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Company_Industry_Id",
                table: "Company",
                column: "Industry_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Industry_Industry_Id",
                table: "Company",
                column: "Industry_Id",
                principalTable: "Industry",
                principalColumn: "Industry_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Industry_Industry_Id",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_Industry_Id",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Industry_Id",
                table: "Company");
        }
    }
}
