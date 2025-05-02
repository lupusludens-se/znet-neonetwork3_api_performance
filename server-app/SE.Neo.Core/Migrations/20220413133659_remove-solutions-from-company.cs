using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class removesolutionsfromcompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company_Solution");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Company management");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company_Solution",
                columns: table => new
                {
                    Company_Solution_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Solution_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Solution", x => x.Company_Solution_Id);
                    table.ForeignKey(
                        name: "FK_Company_Solution_CMS_Solution_Solution_Id",
                        column: x => x.Solution_Id,
                        principalTable: "CMS_Solution",
                        principalColumn: "CMS_Solution_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Solution_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Company add");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Solution_Company_Id",
                table: "Company_Solution",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Solution_Solution_Id",
                table: "Company_Solution",
                column: "Solution_Id");
        }
    }
}
