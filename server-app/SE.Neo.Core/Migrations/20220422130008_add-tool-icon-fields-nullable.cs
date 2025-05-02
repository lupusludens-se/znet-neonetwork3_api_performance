using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addtooliconfieldsnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon_Url",
                table: "Tool");

            migrationBuilder.AddColumn<string>(
                name: "Icon_Name",
                table: "Tool",
                type: "nvarchar(1024)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Icon_Name",
                table: "Tool",
                column: "Icon_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Tool_Blob_Icon_Name",
                table: "Tool",
                column: "Icon_Name",
                principalTable: "Blob",
                principalColumn: "Blob_Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tool_Blob_Icon_Name",
                table: "Tool");

            migrationBuilder.DropIndex(
                name: "IX_Tool_Icon_Name",
                table: "Tool");

            migrationBuilder.DropColumn(
                name: "Icon_Name",
                table: "Tool");

            migrationBuilder.AddColumn<string>(
                name: "Icon_Url",
                table: "Tool",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
