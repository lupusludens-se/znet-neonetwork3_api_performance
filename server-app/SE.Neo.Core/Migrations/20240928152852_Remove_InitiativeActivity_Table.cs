using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class Remove_InitiativeActivity_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Initiative_Activity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Initiative_Activity",
                columns: table => new
                {
                    Initiative_Activity_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Initiative_Module_Id = table.Column<int>(type: "int", nullable: false),
                    Last_Date_Time_Click = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Activity", x => x.Initiative_Activity_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Activity_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Activity_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Activity_Initiative_Id",
                table: "Initiative_Activity",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Activity_User_Id",
                table: "Initiative_Activity",
                column: "User_Id");
        }
    }
}
