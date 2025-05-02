using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedSavedContentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Project_Name",
                table: "Project",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Article_Saved",
                columns: table => new
                {
                    Article_Saved_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Article_CMS_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    SavedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article_Saved", x => x.Article_Saved_Id);
                    table.ForeignKey(
                        name: "FK_Article_Saved_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Saved",
                columns: table => new
                {
                    Project_Saved_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    SavedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Saved", x => x.Project_Saved_Id);
                    table.ForeignKey(
                        name: "FK_Project_Saved_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Saved_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tool_Saved",
                columns: table => new
                {
                    Tool_Saved_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Tool_Id = table.Column<int>(type: "int", nullable: false),
                    SavedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_Article_Saved_User_Id",
                table: "Article_Saved",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Saved_Project_Id",
                table: "Project_Saved",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Saved_User_Id",
                table: "Project_Saved",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Saved_Tool_Id",
                table: "Tool_Saved",
                column: "Tool_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Saved_User_Id",
                table: "Tool_Saved",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article_Saved");

            migrationBuilder.DropTable(
                name: "Project_Saved");

            migrationBuilder.DropTable(
                name: "Tool_Saved");

            migrationBuilder.AlterColumn<string>(
                name: "Project_Name",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);
        }
    }
}
