using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class setuseraddstatusenum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Is_Approved",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Is_Onboarded",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "Status_Id",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "User_Status",
                columns: table => new
                {
                    User_Status_Id = table.Column<int>(type: "int", nullable: false),
                    User_Status_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Status", x => x.User_Status_Id);
                });

            migrationBuilder.InsertData(
                table: "User_Status",
                columns: new[] { "User_Status_Id", "User_Status_Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Denied" },
                    { 3, "Onboard" },
                    { 4, "Active" },
                    { 5, "Inactive" },
                    { 6, "Deleted" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Status_Id",
                table: "User",
                column: "Status_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_Status_Status_Id",
                table: "User",
                column: "Status_Id",
                principalTable: "User_Status",
                principalColumn: "User_Status_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_Status_Status_Id",
                table: "User");

            migrationBuilder.DropTable(
                name: "User_Status");

            migrationBuilder.DropIndex(
                name: "IX_User_Status_Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Status_Id",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Approved",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Onboarded",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
