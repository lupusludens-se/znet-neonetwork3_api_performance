using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addeventinvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event_Invited_User",
                columns: table => new
                {
                    Event_Invited_User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Invited_User", x => x.Event_Invited_User_Id);
                    table.ForeignKey(
                        name: "FK_Event_Invited_User_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Invited_User_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_User_Event_Id_User_Id",
                table: "Event_Invited_User",
                columns: new[] { "Event_Id", "User_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_User_User_Id",
                table: "Event_Invited_User",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event_Invited_User");
        }
    }
}
