using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddEmailAlertTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Email_Alert",
                columns: table => new
                {
                    Email_Alert_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email_Alert", x => x.Email_Alert_Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Email_Alert",
                columns: table => new
                {
                    User_Email_Alert_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Email_Alert_Id = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Email_Alert", x => x.User_Email_Alert_Id);
                    table.ForeignKey(
                        name: "FK_User_Email_Alert_Email_Alert_Email_Alert_Id",
                        column: x => x.Email_Alert_Id,
                        principalTable: "Email_Alert",
                        principalColumn: "Email_Alert_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Email_Alert_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[] { 16, null, null, null, "Email Alert Management", null });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email_Alert_Email_Alert_Id",
                table: "User_Email_Alert",
                column: "Email_Alert_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email_Alert_User_Id",
                table: "User_Email_Alert",
                column: "User_Id");

            // insert default email alerts
            migrationBuilder.Sql("INSERT INTO [dbo].[Email_Alert] ([Title], [Description], [Category], [Frequency]) VALUES " +
                "('Projects', 'Receive summary of Project Catalog, For You projects, changes etc.', 0, 3)," +
                "('Learn', 'Receive summary of new Learn content', 1, 3)," +
                "('Events', 'Event invitations, Event reminders, and when an Event is deleted', 2, 1)," +
                "('Forums', 'Receive summary of new Forum posts', 3, 2)," +
                "('Responses on Forums', 'Receive an email of a response on a Forum you are involved in', 4, 3)," +
                "('Messaging', 'Receive an email when there are new unread messages', 5, 1)");

            migrationBuilder.Sql("IF(EXISTS(SELECT 1 FROM [dbo].[User])) " +
                "INSERT INTO [dbo].[User_Email_Alert] ([User_Id], [Email_Alert_Id], [Frequency]) " +
                "(SELECT [User_Id], [Email_Alert_Id], [Frequency] FROM [dbo].[User] CROSS JOIN [dbo].[Email_Alert])");

            migrationBuilder.Sql("INSERT INTO [dbo].[Role_Permission](Role_Id,Permission_Id) VALUES (1, 16)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Email_Alert");

            migrationBuilder.DropTable(
                name: "Email_Alert");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 16);
        }
    }
}
