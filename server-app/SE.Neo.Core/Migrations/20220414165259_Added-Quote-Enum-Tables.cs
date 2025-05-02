using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedQuoteEnumTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contract_Structures",
                table: "Solar_Quote");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "Solar_Quote");

            migrationBuilder.CreateTable(
                name: "Quote_Contract_Structure",
                columns: table => new
                {
                    Quote_Contract_Structure_Id = table.Column<int>(type: "int", nullable: false),
                    Quote_Contract_Structure_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quote_Contract_Structure", x => x.Quote_Contract_Structure_Id);
                });

            migrationBuilder.CreateTable(
                name: "Quote_Interest",
                columns: table => new
                {
                    Quote_Interest_Id = table.Column<int>(type: "int", nullable: false),
                    Quote_Interest_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quote_Interest", x => x.Quote_Interest_Id);
                });

            migrationBuilder.CreateTable(
                name: "Solar_Quote_Contract_Structure",
                columns: table => new
                {
                    Solar_Contract_Structure_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Solar_Quote_Id = table.Column<int>(type: "int", nullable: false),
                    Quote_Contract_Structure_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solar_Quote_Contract_Structure", x => x.Solar_Contract_Structure_Id);
                    table.ForeignKey(
                        name: "FK_Solar_Quote_Contract_Structure_Quote_Contract_Structure_Quote_Contract_Structure_Id",
                        column: x => x.Quote_Contract_Structure_Id,
                        principalTable: "Quote_Contract_Structure",
                        principalColumn: "Quote_Contract_Structure_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solar_Quote_Contract_Structure_Solar_Quote_Solar_Quote_Id",
                        column: x => x.Solar_Quote_Id,
                        principalTable: "Solar_Quote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solar_Quote_Interest",
                columns: table => new
                {
                    Solar_Quote_Interest_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Solar_Quote_Id = table.Column<int>(type: "int", nullable: false),
                    Quote_Interest_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solar_Quote_Interest", x => x.Solar_Quote_Interest_Id);
                    table.ForeignKey(
                        name: "FK_Solar_Quote_Interest_Quote_Interest_Quote_Interest_Id",
                        column: x => x.Quote_Interest_Id,
                        principalTable: "Quote_Interest",
                        principalColumn: "Quote_Interest_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solar_Quote_Interest_Solar_Quote_Solar_Quote_Id",
                        column: x => x.Solar_Quote_Id,
                        principalTable: "Solar_Quote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Quote_Contract_Structure",
                columns: new[] { "Quote_Contract_Structure_Id", "Quote_Contract_Structure_Name" },
                values: new object[,]
                {
                    { 1, "Cash Purchase" },
                    { 2, "Power Purchase Agreement" },
                    { 3, "Others" }
                });

            migrationBuilder.InsertData(
                table: "Quote_Interest",
                columns: new[] { "Quote_Interest_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Quote_Interest_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Cost Savings", null },
                    { 2, null, null, null, "Environmental Attributes and/or Carbon Reduction Targets", null },
                    { 3, null, null, null, "Story / Publicity", null },
                    { 4, null, null, null, "Resiliency", null },
                    { 5, null, null, null, "Something Else", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solar_Quote_Contract_Structure_Quote_Contract_Structure_Id",
                table: "Solar_Quote_Contract_Structure",
                column: "Quote_Contract_Structure_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solar_Quote_Contract_Structure_Solar_Quote_Id",
                table: "Solar_Quote_Contract_Structure",
                column: "Solar_Quote_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solar_Quote_Interest_Quote_Interest_Id",
                table: "Solar_Quote_Interest",
                column: "Quote_Interest_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solar_Quote_Interest_Solar_Quote_Id",
                table: "Solar_Quote_Interest",
                column: "Solar_Quote_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solar_Quote_Contract_Structure");

            migrationBuilder.DropTable(
                name: "Solar_Quote_Interest");

            migrationBuilder.DropTable(
                name: "Quote_Contract_Structure");

            migrationBuilder.DropTable(
                name: "Quote_Interest");

            migrationBuilder.AddColumn<string>(
                name: "Contract_Structures",
                table: "Solar_Quote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "Solar_Quote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
