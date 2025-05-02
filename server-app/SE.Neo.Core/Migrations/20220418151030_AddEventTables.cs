using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddEventTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Event_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Highlights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Highlighted = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location_Type = table.Column<int>(type: "int", nullable: false),
                    User_Registration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Event_Type = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Event_Id);
                });

            migrationBuilder.CreateTable(
                name: "Event_Category",
                columns: table => new
                {
                    Event_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Category", x => x.Event_Category_Id);
                    table.ForeignKey(
                        name: "FK_Event_Category_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Category_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event_Company",
                columns: table => new
                {
                    Event_Company_Id = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Event_Link",
                columns: table => new
                {
                    Event_Link_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Link", x => x.Event_Link_Id);
                    table.ForeignKey(
                        name: "FK_Event_Link_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event_Moderator",
                columns: table => new
                {
                    Event_Moderator_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Id = table.Column<int>(type: "int", nullable: true),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Moderator", x => x.Event_Moderator_Id);
                    table.ForeignKey(
                        name: "FK_Event_Moderator_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Moderator_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event_Occurrence",
                columns: table => new
                {
                    Event_Occurrence_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time_Zone_Id = table.Column<int>(type: "int", nullable: false),
                    Event_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event_Occurrence", x => x.Event_Occurrence_Id);
                    table.ForeignKey(
                        name: "FK_Event_Occurrence_Event_Event_Id",
                        column: x => x.Event_Id,
                        principalTable: "Event",
                        principalColumn: "Event_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Occurrence_Time_Zone_Time_Zone_Id",
                        column: x => x.Time_Zone_Id,
                        principalTable: "Time_Zone",
                        principalColumn: "Time_Zone_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[] { 34, null, null, null, "Event view all", null });

            migrationBuilder.CreateIndex(
                name: "IX_Event_Category_Category_Id",
                table: "Event_Category",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Category_Event_Id_Category_Id",
                table: "Event_Category",
                columns: new[] { "Event_Id", "Category_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Company_Company_Id",
                table: "Event_Company",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Company_Event_Id_Company_Id",
                table: "Event_Company",
                columns: new[] { "Event_Id", "Company_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Link_Event_Id",
                table: "Event_Link",
                column: "Event_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Moderator_Event_Id",
                table: "Event_Moderator",
                column: "Event_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Moderator_User_Id",
                table: "Event_Moderator",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Occurrence_Event_Id",
                table: "Event_Occurrence",
                column: "Event_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Occurrence_Time_Zone_Id",
                table: "Event_Occurrence",
                column: "Time_Zone_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event_Category");

            migrationBuilder.DropTable(
                name: "Event_Company");

            migrationBuilder.DropTable(
                name: "Event_Link");

            migrationBuilder.DropTable(
                name: "Event_Moderator");

            migrationBuilder.DropTable(
                name: "Event_Occurrence");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 34);
        }
    }
}
