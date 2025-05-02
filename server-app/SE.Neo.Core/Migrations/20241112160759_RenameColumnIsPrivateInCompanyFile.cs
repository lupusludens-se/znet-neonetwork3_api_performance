using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class RenameColumnIsPrivateInCompanyFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "Company_File",
                newName: "Is_Private");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Is_Private",
                table: "Company_File",
                newName: "IsPrivate");
        }
    }
}
