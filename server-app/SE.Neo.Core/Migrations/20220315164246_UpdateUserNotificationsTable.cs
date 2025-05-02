using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdateUserNotificationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Platform_Notification");

            migrationBuilder.CreateTable(
                name: "User_Notification",
                columns: table => new
                {
                    User_Notification_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Notification_Type = table.Column<int>(type: "int", nullable: false),
                    Is_Read = table.Column<bool>(type: "bit", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Notification", x => x.User_Notification_Id);
                    table.ForeignKey(
                        name: "FK_User_Notification_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Notification_User_Id_Is_Read",
                table: "User_Notification",
                columns: new[] { "User_Id", "Is_Read" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Notification");

            migrationBuilder.CreateTable(
                name: "User_Platform_Notification",
                columns: table => new
                {
                    User_Platform_Notification_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Read = table.Column<bool>(type: "bit", nullable: false),
                    Platform_Notification_Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Platform_Notification", x => x.User_Platform_Notification_Id);
                    table.ForeignKey(
                        name: "FK_User_Platform_Notification_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Platform_Notification_User_Id_Is_Read",
                table: "User_Platform_Notification",
                columns: new[] { "User_Id", "Is_Read" });
        }
    }
}
