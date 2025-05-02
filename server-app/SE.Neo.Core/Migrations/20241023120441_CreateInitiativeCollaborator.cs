using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class CreateInitiativeCollaborator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Initiative_Collaborator",
                columns: table => new
                {
                    Initiative_Collaborator_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Collaborator", x => x.Initiative_Collaborator_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Collaborator_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Collaborator_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Collaborator_Initiative_Id",
                table: "Initiative_Collaborator",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Collaborator_User_Id",
                table: "Initiative_Collaborator",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Initiative_Collaborator");
        }
    }
}
