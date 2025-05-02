using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddSavedToArticleRelation_RemoveSavedTools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tool_Saved");

            migrationBuilder.DropColumn(
                name: "SavedDate",
                table: "Project_Saved");

            migrationBuilder.DropColumn(
                name: "SavedDate",
                table: "Discussion_Saved");

            migrationBuilder.DropColumn(
                name: "SavedDate",
                table: "Article_Saved");

            migrationBuilder.CreateIndex(
                name: "IX_Article_Saved_Article_CMS_Id",
                table: "Article_Saved",
                column: "Article_CMS_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Saved_CMS_Article_Article_CMS_Id",
                table: "Article_Saved",
                column: "Article_CMS_Id",
                principalTable: "CMS_Article",
                principalColumn: "CMS_Article_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Saved_CMS_Article_Article_CMS_Id",
                table: "Article_Saved");

            migrationBuilder.DropIndex(
                name: "IX_Article_Saved_Article_CMS_Id",
                table: "Article_Saved");

            migrationBuilder.AddColumn<DateTime>(
                name: "SavedDate",
                table: "Project_Saved",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SavedDate",
                table: "Discussion_Saved",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SavedDate",
                table: "Article_Saved",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Tool_Saved",
                columns: table => new
                {
                    Tool_Saved_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tool_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SavedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool_Saved", x => x.Tool_Saved_Id);
                    table.ForeignKey(
                        name: "FK_Tool_Saved_Tool_Tool_Id",
                        column: x => x.Tool_Id,
                        principalTable: "Tool",
                        principalColumn: "Tool_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tool_Saved_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Saved_Tool_Id",
                table: "Tool_Saved",
                column: "Tool_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Saved_User_Id",
                table: "Tool_Saved",
                column: "User_Id");
        }
    }
}
