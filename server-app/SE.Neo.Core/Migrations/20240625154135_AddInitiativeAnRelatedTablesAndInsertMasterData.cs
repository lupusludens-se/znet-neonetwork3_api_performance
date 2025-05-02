using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddInitiativeAnRelatedTablesAndInsertMasterData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Initiative_Scale",
                columns: table => new
                {
                    Initiative_Scale_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Scale", x => x.Initiative_Scale_Id);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Status",
                columns: table => new
                {
                    Status_Id = table.Column<int>(type: "int", nullable: false),
                    Status_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Status", x => x.Status_Id);
                });

            migrationBuilder.CreateTable(
                name: "Initiative",
                columns: table => new
                {
                    Initiative_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Project_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    StatusIdId = table.Column<int>(type: "int", nullable: false),
                    ScaleIdId = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative", x => x.Initiative_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_CMS_Category_Project_Type_Id",
                        column: x => x.Project_Type_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Initiative_Scale_ScaleIdId",
                        column: x => x.ScaleIdId,
                        principalTable: "Initiative_Scale",
                        principalColumn: "Initiative_Scale_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Initiative_Status_StatusIdId",
                        column: x => x.StatusIdId,
                        principalTable: "Initiative_Status",
                        principalColumn: "Status_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Activity",
                columns: table => new
                {
                    Initiative_Activity_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Last_Date_Time_Click = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Initiative_Module_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Activity", x => x.Initiative_Activity_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Activity_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Activity_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Article",
                columns: table => new
                {
                    Initiative_Article_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    Article_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Article", x => x.Initiative_Article_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Article_CMS_Article_Article_Id",
                        column: x => x.Article_Id,
                        principalTable: "CMS_Article",
                        principalColumn: "CMS_Article_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Article_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Community",
                columns: table => new
                {
                    Initiative_Community_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Community", x => x.Initiative_Community_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Community_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Community_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Discussion",
                columns: table => new
                {
                    Initiative_Discussion_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    Discussion_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Discussion", x => x.Initiative_Discussion_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Discussion_Discussion_Discussion_Id",
                        column: x => x.Discussion_Id,
                        principalTable: "Discussion",
                        principalColumn: "Discussion_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Discussion_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Project",
                columns: table => new
                {
                    Initiative_Project_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Project", x => x.Initiative_Project_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Project_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Project_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Region",
                columns: table => new
                {
                    Initiative_Region_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    CMS_Region_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Region", x => x.Initiative_Region_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Region_CMS_Region_CMS_Region_Id",
                        column: x => x.CMS_Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Region_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Initiative_Tool",
                columns: table => new
                {
                    Initiative_Tool_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initiative_Id = table.Column<int>(type: "int", nullable: false),
                    Tool_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative_Tool", x => x.Initiative_Tool_Id);
                    table.ForeignKey(
                        name: "FK_Initiative_Tool_Initiative_Initiative_Id",
                        column: x => x.Initiative_Id,
                        principalTable: "Initiative",
                        principalColumn: "Initiative_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Initiative_Tool_Tool_Tool_Id",
                        column: x => x.Tool_Id,
                        principalTable: "Tool",
                        principalColumn: "Tool_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Company_Id",
                table: "Initiative",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Project_Type_Id",
                table: "Initiative",
                column: "Project_Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_ScaleIdId",
                table: "Initiative",
                column: "ScaleIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_StatusIdId",
                table: "Initiative",
                column: "StatusIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_User_Id",
                table: "Initiative",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Activity_Initiative_Id_User_Id",
                table: "Initiative_Activity",
                columns: new[] { "Initiative_Id", "User_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Activity_User_Id",
                table: "Initiative_Activity",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Article_Article_Id",
                table: "Initiative_Article",
                column: "Article_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Article_Initiative_Id_Article_Id",
                table: "Initiative_Article",
                columns: new[] { "Initiative_Id", "Article_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Community_Initiative_Id_User_Id",
                table: "Initiative_Community",
                columns: new[] { "Initiative_Id", "User_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Community_User_Id",
                table: "Initiative_Community",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Discussion_Discussion_Id",
                table: "Initiative_Discussion",
                column: "Discussion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Discussion_Initiative_Id_Discussion_Id",
                table: "Initiative_Discussion",
                columns: new[] { "Initiative_Id", "Discussion_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Project_Initiative_Id_Project_Id",
                table: "Initiative_Project",
                columns: new[] { "Initiative_Id", "Project_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Project_Project_Id",
                table: "Initiative_Project",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Region_CMS_Region_Id",
                table: "Initiative_Region",
                column: "CMS_Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Region_Initiative_Id_CMS_Region_Id",
                table: "Initiative_Region",
                columns: new[] { "Initiative_Id", "CMS_Region_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Tool_Initiative_Id_Tool_Id",
                table: "Initiative_Tool",
                columns: new[] { "Initiative_Id", "Tool_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Tool_Tool_Id",
                table: "Initiative_Tool",
                column: "Tool_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Initiative_Activity");

            migrationBuilder.DropTable(
                name: "Initiative_Article");

            migrationBuilder.DropTable(
                name: "Initiative_Community");

            migrationBuilder.DropTable(
                name: "Initiative_Discussion");

            migrationBuilder.DropTable(
                name: "Initiative_Project");

            migrationBuilder.DropTable(
                name: "Initiative_Region");

            migrationBuilder.DropTable(
                name: "Initiative_Tool");

            migrationBuilder.DropTable(
                name: "Initiative");

            migrationBuilder.DropTable(
                name: "Initiative_Scale");

            migrationBuilder.DropTable(
                name: "Initiative_Status");
        }
    }
}
