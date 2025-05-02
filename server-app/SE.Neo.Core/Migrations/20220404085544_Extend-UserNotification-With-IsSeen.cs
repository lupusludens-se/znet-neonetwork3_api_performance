using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class ExtendUserNotificationWithIsSeen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_Seen",
                table: "User_Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_User_Notification_User_Id_Is_Seen",
                table: "User_Notification",
                columns: new[] { "User_Id", "Is_Seen" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Notification_User_Id_Is_Seen",
                table: "User_Notification");

            migrationBuilder.DropColumn(
                name: "Is_Seen",
                table: "User_Notification");
        }
    }
}
