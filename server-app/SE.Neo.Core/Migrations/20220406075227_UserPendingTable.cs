using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UserPendingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_Pending",
                columns: table => new
                {
                    User_Pending_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsDenied = table.Column<bool>(type: "bit", nullable: false),
                    User_Pending_Role_Id = table.Column<int>(type: "int", nullable: false),
                    Company_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Pending_Company_Id = table.Column<int>(type: "int", nullable: false),
                    User_Pending_Country_Id = table.Column<int>(type: "int", nullable: false),
                    User_Pending_Heard_Via_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Pending", x => x.User_Pending_Id);
                    table.ForeignKey(
                        name: "FK_User_Pending_Company_User_Pending_Company_Id",
                        column: x => x.User_Pending_Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Pending_Country_User_Pending_Country_Id",
                        column: x => x.User_Pending_Country_Id,
                        principalTable: "Country",
                        principalColumn: "Country_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Pending_Heard_Via_User_Pending_Heard_Via_Id",
                        column: x => x.User_Pending_Heard_Via_Id,
                        principalTable: "Heard_Via",
                        principalColumn: "Heard_Via_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Pending_Role_User_Pending_Role_Id",
                        column: x => x.User_Pending_Role_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[] { 31, null, null, null, "User admit", null });

            migrationBuilder.CreateIndex(
                name: "IX_User_Pending_Email",
                table: "User_Pending",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Pending_First_Name_Last_Name",
                table: "User_Pending",
                columns: new[] { "First_Name", "Last_Name" });

            migrationBuilder.CreateIndex(
                name: "IX_User_Pending_User_Pending_Company_Id",
                table: "User_Pending",
                column: "User_Pending_Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Pending_User_Pending_Country_Id",
                table: "User_Pending",
                column: "User_Pending_Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Pending_User_Pending_Heard_Via_Id",
                table: "User_Pending",
                column: "User_Pending_Heard_Via_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Pending_User_Pending_Role_Id",
                table: "User_Pending",
                column: "User_Pending_Role_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Pending");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 31);
        }
    }
}
