using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddCompanyAnnouncementTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company_Announcement",
                columns: table => new
                {
                    Company_Announcement_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Status_Id = table.Column<int>(type: "int", nullable: false),
                    Scale_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Announcement", x => x.Company_Announcement_Id);
                    table.ForeignKey(
                        name: "FK_Company_Announcement_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Announcement_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Company_Announcement_Region",
                columns: table => new
                {
                    Company_Announcement_Region_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Announcement_Id = table.Column<int>(type: "int", nullable: false),
                    CMS_Region_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Announcement_Region", x => x.Company_Announcement_Region_Id);
                    table.ForeignKey(
                        name: "FK_Company_Announcement_Region_CMS_Region_CMS_Region_Id",
                        column: x => x.CMS_Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Announcement_Region_Company_Announcement_Company_Announcement_Id",
                        column: x => x.Company_Announcement_Id,
                        principalTable: "Company_Announcement",
                        principalColumn: "Company_Announcement_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_Announcement_Company_Id",
                table: "Company_Announcement",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Announcement_User_Id",
                table: "Company_Announcement",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Announcement_Region_CMS_Region_Id",
                table: "Company_Announcement_Region",
                column: "CMS_Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Announcement_Region_Company_Announcement_Id",
                table: "Company_Announcement_Region",
                column: "Company_Announcement_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company_Announcement_Region");

            migrationBuilder.DropTable(
                name: "Company_Announcement");
        }
    }
}
