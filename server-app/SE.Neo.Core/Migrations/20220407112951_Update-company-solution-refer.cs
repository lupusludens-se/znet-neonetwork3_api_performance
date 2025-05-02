using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class Updatecompanysolutionrefer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company_Solution_Capability");

            migrationBuilder.DropTable(
                name: "Solution_Capability");

            migrationBuilder.CreateTable(
                name: "Company_Solution",
                columns: table => new
                {
                    Company_Solution_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Solution_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Company_Solution_Company_Id",
                table: "Company_Solution",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Solution_Solution_Id",
                table: "Company_Solution",
                column: "Solution_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company_Solution");

            migrationBuilder.CreateTable(
                name: "Solution_Capability",
                columns: table => new
                {
                    Solution_Capability_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Solution_Capability_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solution_Capability", x => x.Solution_Capability_Id);
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
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
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
                name: "IX_Company_Solution_Capability_Company_Id",
                table: "Company_Solution_Capability",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Solution_Capability_Solution_Capability_Id",
                table: "Company_Solution_Capability",
                column: "Solution_Capability_Id");
        }
    }
}
