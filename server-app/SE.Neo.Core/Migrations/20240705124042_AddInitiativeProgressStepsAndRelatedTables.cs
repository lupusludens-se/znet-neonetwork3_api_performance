using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddInitiativeProgressStepsAndRelatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Progress_Details_Initiative_Project_Sub_Step_Initiative_Project_Sub_Step_Id",
                table: "Initiative_Progress_Details");

            migrationBuilder.DropTable(
                name: "Initiative_Project_Sub_Step");

            migrationBuilder.RenameColumn(
                name: "Initiative_Project_Sub_Step_Id",
                table: "Initiative_Progress_Details",
                newName: "Initiative_Sub_Step_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Initiative_Progress_Details_Initiative_Project_Sub_Step_Id",
                table: "Initiative_Progress_Details",
                newName: "IX_Initiative_Progress_Details_Initiative_Sub_Step_Id");

            migrationBuilder.AddColumn<int>(
                name: "Category_Id",
                table: "Initiative_Sub_Step",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Initiative_Step_Id",
                table: "Initiative_Sub_Step",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sub_Step_Order",
                table: "Initiative_Sub_Step",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Initiative_Step_Id",
                table: "Initiative_Progress_Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Sub_Step_Category_Id",
                table: "Initiative_Sub_Step",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Sub_Step_Initiative_Step_Id",
                table: "Initiative_Sub_Step",
                column: "Initiative_Step_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Progress_Details_Initiative_Step_Id",
                table: "Initiative_Progress_Details",
                column: "Initiative_Step_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Progress_Details_Initiative_Step_Initiative_Step_Id",
                table: "Initiative_Progress_Details",
                column: "Initiative_Step_Id",
                principalTable: "Initiative_Step",
                principalColumn: "Initiative_Step_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Progress_Details_Initiative_Sub_Step_Initiative_Sub_Step_Id",
                table: "Initiative_Progress_Details",
                column: "Initiative_Sub_Step_Id",
                principalTable: "Initiative_Sub_Step",
                principalColumn: "Initiative_Sub_Step_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Sub_Step_CMS_Category_Category_Id",
                table: "Initiative_Sub_Step",
                column: "Category_Id",
                principalTable: "CMS_Category",
                principalColumn: "CMS_Category_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Sub_Step_Initiative_Step_Initiative_Step_Id",
                table: "Initiative_Sub_Step",
                column: "Initiative_Step_Id",
                principalTable: "Initiative_Step",
                principalColumn: "Initiative_Step_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Progress_Details_Initiative_Step_Initiative_Step_Id",
                table: "Initiative_Progress_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Progress_Details_Initiative_Sub_Step_Initiative_Sub_Step_Id",
                table: "Initiative_Progress_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Sub_Step_CMS_Category_Category_Id",
                table: "Initiative_Sub_Step");

            migrationBuilder.DropForeignKey(
                name: "FK_Initiative_Sub_Step_Initiative_Step_Initiative_Step_Id",
                table: "Initiative_Sub_Step");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Sub_Step_Category_Id",
                table: "Initiative_Sub_Step");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Sub_Step_Initiative_Step_Id",
                table: "Initiative_Sub_Step");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Progress_Details_Initiative_Step_Id",
                table: "Initiative_Progress_Details");

            migrationBuilder.DropColumn(
                name: "Category_Id",
                table: "Initiative_Sub_Step");

            migrationBuilder.DropColumn(
                name: "Initiative_Step_Id",
                table: "Initiative_Sub_Step");

            migrationBuilder.DropColumn(
                name: "Sub_Step_Order",
                table: "Initiative_Sub_Step");

            migrationBuilder.DropColumn(
                name: "Initiative_Step_Id",
                table: "Initiative_Progress_Details");

            migrationBuilder.RenameColumn(
                name: "Initiative_Sub_Step_Id",
                table: "Initiative_Progress_Details",
                newName: "Initiative_Project_Sub_Step_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Initiative_Progress_Details_Initiative_Sub_Step_Id",
                table: "Initiative_Progress_Details",
                newName: "IX_Initiative_Progress_Details_Initiative_Project_Sub_Step_Id");

            migrationBuilder.CreateTable(
                name: "Initiative_Project_Sub_Step",
                columns: table => new
                {
                    Initiative_Project_Sub_Step_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Initiative_Step_Id = table.Column<int>(type: "int", nullable: false),
                    Initiative_Sub_Step_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sub_Step_Order = table.Column<int>(type: "int", nullable: false),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Project_Sub_Step", x => x.Initiative_Project_Sub_Step_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Project_Sub_Step_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Project_Sub_Step_Initiative_Step_Initiative_Step_Id",
                        column: x => x.Initiative_Step_Id,
                        principalTable: "Initiative_Step",
                        principalColumn: "Initiative_Step_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Project_Sub_Step_Initiative_Sub_Step_Initiative_Sub_Step_Id",
                        column: x => x.Initiative_Sub_Step_Id,
                        principalTable: "Initiative_Sub_Step",
                        principalColumn: "Initiative_Sub_Step_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Project_Sub_Step_Category_Id",
                table: "Initiative_Project_Sub_Step",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Project_Sub_Step_Initiative_Step_Id",
                table: "Initiative_Project_Sub_Step",
                column: "Initiative_Step_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Project_Sub_Step_Initiative_Sub_Step_Id",
                table: "Initiative_Project_Sub_Step",
                column: "Initiative_Sub_Step_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Initiative_Progress_Details_Initiative_Project_Sub_Step_Initiative_Project_Sub_Step_Id",
                table: "Initiative_Progress_Details",
                column: "Initiative_Project_Sub_Step_Id",
                principalTable: "Initiative_Project_Sub_Step",
                principalColumn: "Initiative_Project_Sub_Step_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
