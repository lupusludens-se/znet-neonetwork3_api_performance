using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddActualFileTitleColumnInFileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "File_Name",
                table: "File",
                type: "nvarchar(1024)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Actual_File_Name",
                table: "File",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Actual_File_Title",
                table: "File",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actual_File_Title",
                table: "File");

            migrationBuilder.AlterColumn<string>(
                name: "File_Name",
                table: "File",
                type: "nvarchar(1024)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)");

            migrationBuilder.AlterColumn<string>(
                name: "Actual_File_Name",
                table: "File",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
