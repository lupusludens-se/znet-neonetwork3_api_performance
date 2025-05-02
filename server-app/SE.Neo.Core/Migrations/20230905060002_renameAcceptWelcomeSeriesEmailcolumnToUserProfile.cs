using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class renameAcceptWelcomeSeriesEmailcolumnToUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AcceptWelcomeSeriesEmail",
                table: "User_Profile",
                newName: "Accept_Welcome_Series_Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Accept_Welcome_Series_Email",
                table: "User_Profile",
                newName: "AcceptWelcomeSeriesEmail");
        }
    }
}
