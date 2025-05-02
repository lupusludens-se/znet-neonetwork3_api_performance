using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class add_tool_pinned_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tool_Pinned",
                columns: table => new
                {
                    Tool_Pinned_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Tool_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool_Pinned", x => x.Tool_Pinned_Id);
                    table.ForeignKey(
                        name: "FK_Tool_Pinned_Tool_Tool_Id",
                        column: x => x.Tool_Id,
                        principalTable: "Tool",
                        principalColumn: "Tool_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tool_Pinned_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Pinned_Tool_Id",
                table: "Tool_Pinned",
                column: "Tool_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Pinned_User_Id",
                table: "Tool_Pinned",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tool_Pinned");
        }
    }
}
