using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addfilestablemigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    File_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    File_Type = table.Column<int>(type: "int", nullable: false),
                    File_Extension = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_Name = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    Actual_File_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.File_Id);
                    table.ForeignKey(
                        name: "FK_File_Blob_File_Name",
                        column: x => x.File_Name,
                        principalTable: "Blob",
                        principalColumn: "Blob_Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_File",
                columns: table => new
                {
                    Initiative_File_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    File_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_File", x => x.Initiative_File_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_File_File_File_Id",
                        column: x => x.File_Id,
                        principalTable: "File",
                        principalColumn: "File_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_File_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_File_Name",
                table: "File",
                column: "File_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_File_File_Id",
                table: "Initiative_File",
                column: "File_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_File_Initiative_Id",
                table: "Initiative_File",
                column: "Initiative_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Initiative_File");

            migrationBuilder.DropTable(
                name: "File");
        }
    }
}
