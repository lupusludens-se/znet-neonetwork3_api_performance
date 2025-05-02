using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class removeeventinvitedcompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event_Invited_Company");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event_Invited_Company",
                columns: table => new
                {
                    Event_Invited_Company_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Invited_Company", x => x.Event_Invited_Company_Id);
                    table.ForeignKey(
                        name: "FK_Event_Invited_Company_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Invited_Company_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Company_Company_Id",
                table: "Event_Invited_Company",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Company_Event_Id_Company_Id",
                table: "Event_Invited_Company",
                columns: new[] { "Event_Id", "Company_Id" },
                unique: true);
        }
    }
}
