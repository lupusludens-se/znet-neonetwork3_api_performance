using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedDiscussionSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Discussion_Source_Type",
                table: "Discussion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Discussion_Source",
                columns: table => new
                {
                    Discussion_Source_Id = table.Column<int>(type: "int", nullable: false),
                    Discussion_Source_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion_Source", x => x.Discussion_Source_Id);
                });

            migrationBuilder.InsertData(
                table: "Discussion_Source",
                columns: new[] { "Discussion_Source_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Discussion_Source_Name", "Updated_User_Id" },
                values: new object[] { 1, null, null, null, "General", null });

            migrationBuilder.InsertData(
                table: "Discussion_Source",
                columns: new[] { "Discussion_Source_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Discussion_Source_Name", "Updated_User_Id" },
                values: new object[] { 2, null, null, null, "Provider Contact", null });

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Discussion_Source_Type",
                table: "Discussion",
                column: "Discussion_Source_Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussion_Discussion_Source_Discussion_Source_Type",
                table: "Discussion",
                column: "Discussion_Source_Type",
                principalTable: "Discussion_Source",
                principalColumn: "Discussion_Source_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussion_Discussion_Source_Discussion_Source_Type",
                table: "Discussion");

            migrationBuilder.DropTable(
                name: "Discussion_Source");

            migrationBuilder.DropIndex(
                name: "IX_Discussion_Discussion_Source_Type",
                table: "Discussion");

            migrationBuilder.DropColumn(
                name: "Discussion_Source_Type",
                table: "Discussion");
        }
    }
}
