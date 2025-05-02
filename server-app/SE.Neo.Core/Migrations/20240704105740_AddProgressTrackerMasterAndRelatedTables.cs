using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddProgressTrackerMasterAndRelatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Initiative_Step",
                columns: table => new
                {
                    Initiative_Step_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Step_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Step_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Step", x => x.Initiative_Step_Id);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Sub_Step",
                columns: table => new
                {
                    Initiative_Sub_Step_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Sub_Step", x => x.Initiative_Sub_Step_Id);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Project_Sub_Step",
                columns: table => new
                {
                    Initiative_Project_Sub_Step_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Initiative_Step_Id = table.Column<int>(type: "int", nullable: false),
                    Initiative_Sub_Step_Id = table.Column<int>(type: "int", nullable: false),
                    Sub_Step_Order = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Initiative_Progress_Details",
                columns: table => new
                {
                    Initiative_Progress_Details_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    Initiative_Project_Sub_Step_Id = table.Column<int>(type: "int", nullable: false),
                    Is_Checked = table.Column<bool>(type: "bit", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Progress_Details", x => x.Initiative_Progress_Details_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Progress_Details_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Progress_Details_Initiative_Project_Sub_Step_Initiative_Project_Sub_Step_Id",
                        column: x => x.Initiative_Project_Sub_Step_Id,
                        principalTable: "Initiative_Project_Sub_Step",
                        principalColumn: "Initiative_Project_Sub_Step_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Progress_Details_Initiative_Id",
                table: "Initiative_Progress_Details",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Progress_Details_Initiative_Project_Sub_Step_Id",
                table: "Initiative_Progress_Details",
                column: "Initiative_Project_Sub_Step_Id");

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

            migrationBuilder.Sql("Insert Into Initiative_Step  (Step_Name, Step_Description) Values ('Step 1', 'Planning, Data Collection & Education'),('Step 2', 'Internal Alignment'),('Step 3', 'Identify Providers & Collect Offers'),('Step 4', 'Evaluate Offers & Choose Preliminary Partner'),('Step 5', 'Negotiate, Refine Terms & Contract'),('Step 6', 'Execute')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Initiative_Progress_Details");

            migrationBuilder.DropTable(
                name: "Initiative_Project_Sub_Step");

            migrationBuilder.DropTable(
                name: "Initiative_Step");

            migrationBuilder.DropTable(
                name: "Initiative_Sub_Step");
        }
    }
}
