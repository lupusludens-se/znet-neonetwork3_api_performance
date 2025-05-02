using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddColumnCurrentStepInInitiative : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Current_Step_Id",
                table: "Initiative",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Current_Step_Id",
                table: "Initiative",
                column: "Current_Step_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Initiative_Step_Current_Step_Id",
                table: "Initiative",
                column: "Current_Step_Id",
                principalTable: "Initiative_Step",
                principalColumn: "Initiative_Step_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Initiative_Step_Current_Step_Id",
                table: "Initiative");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Current_Step_Id",
                table: "Initiative");

            migrationBuilder.DropColumn(
                name: "Current_Step_Id",
                table: "Initiative");
        }
    }
}
