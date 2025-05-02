using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class UpdatedActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Activity]");

            migrationBuilder.RenameColumn(
                name: "Activity_Type",
                table: "Activity",
                newName: "Activity_Type_Id");

            migrationBuilder.RenameColumn(
                name: "Activity_Page",
                table: "Activity",
                newName: "Activity_Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_Activity_Location_Id",
                table: "Activity",
                column: "Activity_Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_Activity_Type_Id",
                table: "Activity",
                column: "Activity_Type_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Activity_Location_Activity_Location_Id",
                table: "Activity",
                column: "Activity_Location_Id",
                principalTable: "Activity_Location",
                principalColumn: "Activitity_Location_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Activity_Type_Activity_Type_Id",
                table: "Activity",
                column: "Activity_Type_Id",
                principalTable: "Activity_Type",
                principalColumn: "Activitity_Type_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Activity_Location_Activity_Location_Id",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Activity_Type_Activity_Type_Id",
                table: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_Activity_Activity_Location_Id",
                table: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_Activity_Activity_Type_Id",
                table: "Activity");

            migrationBuilder.RenameColumn(
                name: "Activity_Type_Id",
                table: "Activity",
                newName: "Activity_Type");

            migrationBuilder.RenameColumn(
                name: "Activity_Location_Id",
                table: "Activity",
                newName: "Activity_Page");
        }
    }
}
