using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addcompanyfallowerenumstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Company",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Company",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Company",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Company",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Company",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Company_Follower",
                columns: table => new
                {
                    Company_Follower_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Follower_Id = table.Column<int>(type: "int", nullable: false),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Follower", x => x.Company_Follower_Id);
                    table.ForeignKey(
                        name: "FK_Company_Follower_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Follower_User_Follower_Id",
                        column: x => x.Follower_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Offsite_PPA",
                columns: table => new
                {
                    Offsite_PPA_Id = table.Column<int>(type: "int", nullable: false),
                    Offsite_PPA_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offsite_PPA", x => x.Offsite_PPA_Id);
                });

            migrationBuilder.CreateTable(
                name: "Solution_Capability",
                columns: table => new
                {
                    Solution_Capability_Id = table.Column<int>(type: "int", nullable: false),
                    Solution_Capability_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solution_Capability", x => x.Solution_Capability_Id);
                });

            migrationBuilder.CreateTable(
                name: "Company_Offsite_PPA",
                columns: table => new
                {
                    Company_Offsite_PPA_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Offsite_PPA_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Offsite_PPA", x => x.Company_Offsite_PPA_Id);
                    table.ForeignKey(
                        name: "FK_Company_Offsite_PPA_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Offsite_PPA_Offsite_PPA_Offsite_PPA_Id",
                        column: x => x.Offsite_PPA_Id,
                        principalTable: "Offsite_PPA",
                        principalColumn: "Offsite_PPA_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Company_Solution_Capability",
                columns: table => new
                {
                    Company_Solution_Capability_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Solution_Capability_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Solution_Capability", x => x.Company_Solution_Capability_Id);
                    table.ForeignKey(
                        name: "FK_Company_Solution_Capability_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Solution_Capability_Solution_Capability_Solution_Capability_Id",
                        column: x => x.Solution_Capability_Id,
                        principalTable: "Solution_Capability",
                        principalColumn: "Solution_Capability_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Offsite_PPA",
                columns: new[] { "Offsite_PPA_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Offsite_PPA_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Project Development", null },
                    { 2, null, null, null, "Ownership during Construction", null },
                    { 3, null, null, null, "O&M Activities", null },
                    { 4, null, null, null, "Long Term Ownership Interest", null },
                    { 5, null, null, null, "Technology Diversification", null },
                    { 6, null, null, null, "Balancing Party", null },
                    { 7, null, null, null, "Retail / Integrated Company", null }
                });

            migrationBuilder.InsertData(
                table: "Solution_Capability",
                columns: new[] { "Solution_Capability_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Solution_Capability_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Aggregated PPAs", null },
                    { 2, null, null, null, "Battery Storage", null },
                    { 3, null, null, null, "Carbon Offset Purchasing", null },
                    { 4, null, null, null, "Community Solar", null },
                    { 5, null, null, null, "EAC Purchasing", null },
                    { 6, null, null, null, "Efficiency Audits & Consulting", null },
                    { 7, null, null, null, "Efficiency Equipment Measures", null },
                    { 8, null, null, null, "EV Charging & Fleet Electrification", null },
                    { 9, null, null, null, "Fuel Cells", null },
                    { 10, null, null, null, "Offsite Power Purchase Agreement", null },
                    { 11, null, null, null, "Onsite Emerging Technologies", null },
                    { 12, null, null, null, "Renewable Retail Electricity", null },
                    { 13, null, null, null, "Utility Green Tariff", null },
                    { 14, null, null, null, "Onsite Solar", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_Follower_Company_Id",
                table: "Company_Follower",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Follower_Follower_Id",
                table: "Company_Follower",
                column: "Follower_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Offsite_PPA_Company_Id",
                table: "Company_Offsite_PPA",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Offsite_PPA_Offsite_PPA_Id",
                table: "Company_Offsite_PPA",
                column: "Offsite_PPA_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Solution_Capability_Company_Id",
                table: "Company_Solution_Capability",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Solution_Capability_Solution_Capability_Id",
                table: "Company_Solution_Capability",
                column: "Solution_Capability_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company_Follower");

            migrationBuilder.DropTable(
                name: "Company_Offsite_PPA");

            migrationBuilder.DropTable(
                name: "Company_Solution_Capability");

            migrationBuilder.DropTable(
                name: "Offsite_PPA");

            migrationBuilder.DropTable(
                name: "Solution_Capability");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Company");
        }
    }
}
