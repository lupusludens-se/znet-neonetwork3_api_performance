using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddEventInvitedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event_Company");

            migrationBuilder.DropColumn(
                name: "Event_Type",
                table: "Event");

            migrationBuilder.CreateTable(
                name: "Event_Invited_Category",
                columns: table => new
                {
                    Event_Invited_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Invited_Category", x => x.Event_Invited_Category_Id);
                    table.ForeignKey(
                        name: "FK_Event_Invited_Category_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Invited_Category_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event_Invited_Company",
                columns: table => new
                {
                    Event_Invited_Company_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Event_Invited_Region",
                columns: table => new
                {
                    Event_Invited_Region_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Region_Id = table.Column<int>(type: "int", nullable: false),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Invited_Region", x => x.Event_Invited_Region_Id);
                    table.ForeignKey(
                        name: "FK_Event_Invited_Region_CMS_Region_Region_Id",
                        column: x => x.Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Invited_Region_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event_Invited_Role",
                columns: table => new
                {
                    Event_Invited_Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Id = table.Column<int>(type: "int", nullable: false),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Invited_Role", x => x.Event_Invited_Role_Id);
                    table.ForeignKey(
                        name: "FK_Event_Invited_Role_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Invited_Role_Role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Category_Category_Id",
                table: "Event_Invited_Category",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Category_Event_Id_Category_Id",
                table: "Event_Invited_Category",
                columns: new[] { "Event_Id", "Category_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Company_Company_Id",
                table: "Event_Invited_Company",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Company_Event_Id_Company_Id",
                table: "Event_Invited_Company",
                columns: new[] { "Event_Id", "Company_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Region_Event_Id_Region_Id",
                table: "Event_Invited_Region",
                columns: new[] { "Event_Id", "Region_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Region_Region_Id",
                table: "Event_Invited_Region",
                column: "Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Role_Event_Id_Role_Id",
                table: "Event_Invited_Role",
                columns: new[] { "Event_Id", "Role_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Invited_Role_Role_Id",
                table: "Event_Invited_Role",
                column: "Role_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event_Invited_Category");

            migrationBuilder.DropTable(
                name: "Event_Invited_Company");

            migrationBuilder.DropTable(
                name: "Event_Invited_Region");

            migrationBuilder.DropTable(
                name: "Event_Invited_Role");

            migrationBuilder.AddColumn<int>(
                name: "Event_Type",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Event_Company",
                columns: table => new
                {
                    Event_Company_Id = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK_Event_Company", x => x.Event_Company_Id);
                    table.ForeignKey(
                        name: "FK_Event_Company_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Company_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_Company_Company_Id",
                table: "Event_Company",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Company_Event_Id_Company_Id",
                table: "Event_Company",
                columns: new[] { "Event_Id", "Company_Id" },
                unique: true);
        }
    }
}
