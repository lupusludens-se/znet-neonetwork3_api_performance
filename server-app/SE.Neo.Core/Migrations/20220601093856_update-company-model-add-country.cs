using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class updatecompanymodeladdcountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project_Location");

            migrationBuilder.DropTable(
                name: "Location");

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

            migrationBuilder.AddColumn<int>(
                name: "Country_Id",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Company_Country_Id",
                table: "Company",
                column: "Country_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Country_Country_Id",
                table: "Company",
                column: "Country_Id",
                principalTable: "Country",
                principalColumn: "Country_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Country_Country_Id",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_Country_Id",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Country_Id",
                table: "Company");

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
                name: "Location",
                columns: table => new
                {
                    Location_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Continent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<Point>(type: "geography", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Location_Id);
                });

            migrationBuilder.CreateTable(
                name: "Project_Location",
                columns: table => new
                {
                    Project_Location_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location_Id = table.Column<int>(type: "int", nullable: false),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Location", x => x.Project_Location_Id);
                    table.ForeignKey(
                        name: "FK_Project_Location_Location_Location_Id",
                        column: x => x.Location_Id,
                        principalTable: "Location",
                        principalColumn: "Location_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Location_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Location_Location_Id",
                table: "Project_Location",
                column: "Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Location_Project_Id",
                table: "Project_Location",
                column: "Project_Id");
        }
    }
}
