using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class add_solar_quote_request_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tool_Type",
                table: "Tool",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Solar_Quote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Site_Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Annual_Power = table.Column<int>(type: "int", nullable: false),
                    Building_Owned = table.Column<bool>(type: "bit", nullable: false),
                    Roof_Available = table.Column<bool>(type: "bit", nullable: false),
                    Roof_Area = table.Column<int>(type: "int", nullable: false),
                    Roof_Area_Type = table.Column<int>(type: "int", nullable: false),
                    Land_Available = table.Column<bool>(type: "bit", nullable: false),
                    Land_Area = table.Column<int>(type: "int", nullable: false),
                    Land_Area_Type = table.Column<int>(type: "int", nullable: false),
                    Carport_Available = table.Column<bool>(type: "bit", nullable: false),
                    Carport_Area = table.Column<int>(type: "int", nullable: false),
                    Carport_Area_Type = table.Column<int>(type: "int", nullable: false),
                    Contract_Structures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Additional_Comments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Portfolio_Review = table.Column<bool>(type: "bit", nullable: false),
                    Requested_By_User_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solar_Quote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solar_Quote_User_Requested_By_User_Id",
                        column: x => x.Requested_By_User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solar_Quote_Requested_By_User_Id",
                table: "Solar_Quote",
                column: "Requested_By_User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solar_Quote");

            migrationBuilder.DropColumn(
                name: "Tool_Type",
                table: "Tool");
        }
    }
}
