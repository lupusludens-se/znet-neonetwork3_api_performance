using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class remove_user_solution_techology_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Profile_Solution");

            migrationBuilder.DropTable(
                name: "User_Profile_Technology");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_Profile_Solution",
                columns: table => new
                {
                    User_Profile_Solution_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Solution_Id = table.Column<int>(type: "int", nullable: false),
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profile_Solution", x => x.User_Profile_Solution_Id);
                    table.ForeignKey(
                        name: "FK_User_Profile_Solution_CMS_Solution_Solution_Id",
                        column: x => x.Solution_Id,
                        principalTable: "CMS_Solution",
                        principalColumn: "CMS_Solution_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Profile_Solution_User_Profile_User_Profile_Id",
                        column: x => x.User_Profile_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Profile_Technology",
                columns: table => new
                {
                    User_Profile_Technology_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Technology_Id = table.Column<int>(type: "int", nullable: false),
                    User_Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profile_Technology", x => x.User_Profile_Technology_Id);
                    table.ForeignKey(
                        name: "FK_User_Profile_Technology_CMS_Technology_Technology_Id",
                        column: x => x.Technology_Id,
                        principalTable: "CMS_Technology",
                        principalColumn: "CMS_Technology_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Profile_Technology_User_Profile_User_Profile_Id",
                        column: x => x.User_Profile_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Solution_Solution_Id",
                table: "User_Profile_Solution",
                column: "Solution_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Solution_User_Profile_Id",
                table: "User_Profile_Solution",
                column: "User_Profile_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Technology_Technology_Id",
                table: "User_Profile_Technology",
                column: "Technology_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_Technology_User_Profile_Id",
                table: "User_Profile_Technology",
                column: "User_Profile_Id");
        }
    }
}
