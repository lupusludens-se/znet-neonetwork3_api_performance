using System;
using Microsoft.EntityFrameworkCore.Migrations;
using SE.Neo.Common.Enums;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class Usersskillsandcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Skill_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Skill_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleType = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Skill_Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills_By_Category",
                columns: table => new
                {
                    Skill_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Skill_Id = table.Column<int>(type: "int", nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills_By_Category", x => x.Skill_Category_Id);
                    table.ForeignKey(
                        name: "FK_Skills_By_Category_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Skills_By_Category_Skills_Skill_Id",
                        column: x => x.Skill_Id,
                        principalTable: "Skills",
                        principalColumn: "Skill_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Skills_By_Category",
                columns: table => new
                {
                    User_Profile_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Skill_Id = table.Column<int>(type: "int", nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Skills_By_Category", x => x.User_Profile_Category_Id);
                    table.ForeignKey(
                        name: "FK_User_Skills_By_Category_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Skills_By_Category_Skills_Skill_Id",
                        column: x => x.Skill_Id,
                        principalTable: "Skills",
                        principalColumn: "Skill_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Skills_By_Category_User_Profile_User_Profile_Id",
                        column: x => x.User_Profile_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_By_Category_Category_Id",
                table: "Skills_By_Category",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_By_Category_Skill_Id",
                table: "Skills_By_Category",
                column: "Skill_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Skills_By_Category_Category_Id",
                table: "User_Skills_By_Category",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Skills_By_Category_Skill_Id",
                table: "User_Skills_By_Category",
                column: "Skill_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Skills_By_Category_User_Profile_Id",
                table: "User_Skills_By_Category",
                column: "User_Profile_Id");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skills_By_Category");

            migrationBuilder.DropTable(
                name: "User_Skills_By_Category");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
