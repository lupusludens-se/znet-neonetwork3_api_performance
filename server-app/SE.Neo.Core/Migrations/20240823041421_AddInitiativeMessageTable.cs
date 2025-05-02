using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddInitiativeMessageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Initiative_Discussion");

            migrationBuilder.CreateTable(
                name: "Initiative_Conversation",
                columns: table => new
                {
                    Initiative_Conversation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    Discussion_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Conversation", x => x.Initiative_Conversation_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Conversation_Discussion_Discussion_Id",
                        column: x => x.Discussion_Id,
                        principalTable: "Discussion",
                        principalColumn: "Discussion_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Conversation_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Recommendation_Activity_Initiative_Id",
                table: "Initiative_Recommendation_Activity",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Conversation_Discussion_Id",
                table: "Initiative_Conversation",
                column: "Discussion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Conversation_Initiative_Id",
                table: "Initiative_Conversation",
                column: "Initiative_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Recommendation_Activity_Initiative_Initiative_Id",
                table: "Initiative_Recommendation_Activity",
                column: "Initiative_Id",
                principalTable: "Initiative",
                principalColumn: "Initiative_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Recommendation_Activity_Initiative_Initiative_Id",
                table: "Initiative_Recommendation_Activity");

            migrationBuilder.DropTable(
                name: "Initiative_Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Recommendation_Activity_Initiative_Id",
                table: "Initiative_Recommendation_Activity");

            migrationBuilder.CreateTable(
                name: "Initiative_Discussion",
                columns: table => new
                {
                    Initiative_Discussion_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discussion_Id = table.Column<int>(type: "int", nullable: false),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Discussion", x => x.Initiative_Discussion_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Discussion_Discussion_Discussion_Id",
                        column: x => x.Discussion_Id,
                        principalTable: "Discussion",
                        principalColumn: "Discussion_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Discussion_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Discussion_Discussion_Id",
                table: "Initiative_Discussion",
                column: "Discussion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Discussion_Initiative_Id",
                table: "Initiative_Discussion",
                column: "Initiative_Id");
        }
    }
}
