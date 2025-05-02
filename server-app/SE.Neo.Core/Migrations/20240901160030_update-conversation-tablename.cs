using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updateconversationtablename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Conversation_Discussion_Discussion_Id",
                table: "Initiative_Conversation");

            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Conversation_Initiative_Initiative_Id",
                table: "Initiative_Conversation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Initiative_Conversation",
                table: "Initiative_Conversation");

            migrationBuilder.RenameTable(
                name: "Initiative_Conversation",
                newName: "Initiative_Discussion");

            migrationBuilder.RenameColumn(
                name: "Initiative_Conversation_Id",
                table: "Initiative_Discussion",
                newName: "Initiative_Discussion_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Initiative_Conversation_Initiative_Id",
                table: "Initiative_Discussion",
                newName: "IX_Initiative_Discussion_Initiative_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Initiative_Conversation_Discussion_Id",
                table: "Initiative_Discussion",
                newName: "IX_Initiative_Discussion_Discussion_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Initiative_Discussion",
                table: "Initiative_Discussion",
                column: "Initiative_Discussion_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Discussion_Discussion_Discussion_Id",
                table: "Initiative_Discussion",
                column: "Discussion_Id",
                principalTable: "Discussion",
                principalColumn: "Discussion_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Discussion_Initiative_Initiative_Id",
                table: "Initiative_Discussion",
                column: "Initiative_Id",
                principalTable: "Initiative",
                principalColumn: "Initiative_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Discussion_Discussion_Discussion_Id",
                table: "Initiative_Discussion");

            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Discussion_Initiative_Initiative_Id",
                table: "Initiative_Discussion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Initiative_Discussion",
                table: "Initiative_Discussion");

            migrationBuilder.RenameTable(
                name: "Initiative_Discussion",
                newName: "Initiative_Conversation");

            migrationBuilder.RenameColumn(
                name: "Initiative_Discussion_Id",
                table: "Initiative_Conversation",
                newName: "Initiative_Conversation_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Initiative_Discussion_Initiative_Id",
                table: "Initiative_Conversation",
                newName: "IX_Initiative_Conversation_Initiative_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Initiative_Discussion_Discussion_Id",
                table: "Initiative_Conversation",
                newName: "IX_Initiative_Conversation_Discussion_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Initiative_Conversation",
                table: "Initiative_Conversation",
                column: "Initiative_Conversation_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Conversation_Discussion_Discussion_Id",
                table: "Initiative_Conversation",
                column: "Discussion_Id",
                principalTable: "Discussion",
                principalColumn: "Discussion_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Conversation_Initiative_Initiative_Id",
                table: "Initiative_Conversation",
                column: "Initiative_Id",
                principalTable: "Initiative",
                principalColumn: "Initiative_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
