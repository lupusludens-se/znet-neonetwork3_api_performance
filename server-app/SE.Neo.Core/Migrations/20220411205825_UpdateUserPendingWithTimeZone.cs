using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateUserPendingWithTimeZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_Pending_Time_Zone_Id",
                table: "User_Pending",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_Pending_User_Pending_Time_Zone_Id",
                table: "User_Pending",
                column: "User_Pending_Time_Zone_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Pending_Time_Zone_User_Pending_Time_Zone_Id",
                table: "User_Pending",
                column: "User_Pending_Time_Zone_Id",
                principalTable: "Time_Zone",
                principalColumn: "Time_Zone_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Pending_Time_Zone_User_Pending_Time_Zone_Id",
                table: "User_Pending");

            migrationBuilder.DropIndex(
                name: "IX_User_Pending_User_Pending_Time_Zone_Id",
                table: "User_Pending");

            migrationBuilder.DropColumn(
                name: "User_Pending_Time_Zone_Id",
                table: "User_Pending");
        }
    }
}
