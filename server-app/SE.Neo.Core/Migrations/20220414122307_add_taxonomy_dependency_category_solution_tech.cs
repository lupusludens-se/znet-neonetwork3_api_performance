using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class add_taxonomy_dependency_category_solution_tech : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CMS_Solution_Id",
                table: "CMS_Category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CMS_Category_Technology",
                columns: table => new
                {
                    CMS_Category_Technology_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMS_Category_Id = table.Column<int>(type: "int", nullable: false),
                    CMS_Technology_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Category_Technology", x => x.CMS_Category_Technology_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Category_Technology_CMS_Category_CMS_Category_Id",
                        column: x => x.CMS_Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CMS_Category_Technology_CMS_Technology_CMS_Technology_Id",
                        column: x => x.CMS_Technology_Id,
                        principalTable: "CMS_Technology",
                        principalColumn: "CMS_Technology_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Category_CMS_Solution_Id",
                table: "CMS_Category",
                column: "CMS_Solution_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Category_Technology_CMS_Category_Id",
                table: "CMS_Category_Technology",
                column: "CMS_Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Category_Technology_CMS_Technology_Id",
                table: "CMS_Category_Technology",
                column: "CMS_Technology_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CMS_Category_CMS_Solution_CMS_Solution_Id",
                table: "CMS_Category",
                column: "CMS_Solution_Id",
                principalTable: "CMS_Solution",
                principalColumn: "CMS_Solution_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMS_Category_CMS_Solution_CMS_Solution_Id",
                table: "CMS_Category");

            migrationBuilder.DropTable(
                name: "CMS_Category_Technology");

            migrationBuilder.DropIndex(
                name: "IX_CMS_Category_CMS_Solution_Id",
                table: "CMS_Category");

            migrationBuilder.DropColumn(
                name: "CMS_Solution_Id",
                table: "CMS_Category");
        }
    }
}
