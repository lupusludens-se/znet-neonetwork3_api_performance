using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class FixedEventTimeZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Occurrence_Time_Zone_Time_Zone_Id",
                table: "Event_Occurrence");

            migrationBuilder.DropIndex(
                name: "IX_Event_Occurrence_Time_Zone_Id",
                table: "Event_Occurrence");

            migrationBuilder.DropColumn(
                name: "Time_Zone_Id",
                table: "Event_Occurrence");

            migrationBuilder.AddColumn<int>(
                name: "Time_Zone_Id",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Time_Zone_Id",
                table: "Event",
                column: "Time_Zone_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Time_Zone_Time_Zone_Id",
                table: "Event",
                column: "Time_Zone_Id",
                principalTable: "Time_Zone",
                principalColumn: "Time_Zone_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Time_Zone_Time_Zone_Id",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_Time_Zone_Id",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Time_Zone_Id",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "Time_Zone_Id",
                table: "Event_Occurrence",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Occurrence_Time_Zone_Id",
                table: "Event_Occurrence",
                column: "Time_Zone_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Occurrence_Time_Zone_Time_Zone_Id",
                table: "Event_Occurrence",
                column: "Time_Zone_Id",
                principalTable: "Time_Zone",
                principalColumn: "Time_Zone_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
