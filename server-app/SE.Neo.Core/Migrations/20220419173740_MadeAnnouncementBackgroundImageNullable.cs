using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class MadeAnnouncementBackgroundImageNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Background_Image_Name",
                table: "Announcement",
                type: "nvarchar(1024)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Background_Image_Name",
                table: "Announcement",
                type: "nvarchar(1024)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldNullable: true);
        }
    }
}
