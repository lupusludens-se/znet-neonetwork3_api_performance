using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddColumn_User_UserPending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminComments",
                table: "User_Pending",
                type: "nvarchar(max)",
                maxLength: 12000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminComments",
                table: "User",
                type: "nvarchar(max)",
                maxLength: 12000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminComments",
                table: "User_Pending");

            migrationBuilder.DropColumn(
                name: "AdminComments",
                table: "User");
        }
    }
}
