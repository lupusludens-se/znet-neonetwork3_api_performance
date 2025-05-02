using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class addarticledependenies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CMS_Article_Type",
                columns: table => new
                {
                    Article_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Article_Type_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Article_Type", x => x.Article_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Article",
                columns: table => new
                {
                    CMS_Article_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Is_Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Article_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Video_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Article", x => x.CMS_Article_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Article_CMS_Article_Type_Type_Id",
                        column: x => x.Type_Id,
                        principalTable: "CMS_Article_Type",
                        principalColumn: "Article_Type_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Article_Category",
                columns: table => new
                {
                    CMS_Article_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMS_Article_Id = table.Column<int>(type: "int", nullable: false),
                    CMS_Category_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Article_Category", x => x.CMS_Article_Category_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Category_CMS_Article_CMS_Article_Id",
                        column: x => x.CMS_Article_Id,
                        principalTable: "CMS_Article",
                        principalColumn: "CMS_Article_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Category_CMS_Category_CMS_Category_Id",
                        column: x => x.CMS_Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Article_Region",
                columns: table => new
                {
                    CMS_Article_Region_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMS_Article_Id = table.Column<int>(type: "int", nullable: false),
                    CMS_Region_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Article_Region", x => x.CMS_Article_Region_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Region_CMS_Article_CMS_Article_Id",
                        column: x => x.CMS_Article_Id,
                        principalTable: "CMS_Article",
                        principalColumn: "CMS_Article_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Region_CMS_Region_CMS_Region_Id",
                        column: x => x.CMS_Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Article_Solution",
                columns: table => new
                {
                    CMS_Article_Solution_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMS_Article_Id = table.Column<int>(type: "int", nullable: false),
                    CMS_Solution_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Article_Solution", x => x.CMS_Article_Solution_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Solution_CMS_Article_CMS_Article_Id",
                        column: x => x.CMS_Article_Id,
                        principalTable: "CMS_Article",
                        principalColumn: "CMS_Article_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Solution_CMS_Solution_CMS_Solution_Id",
                        column: x => x.CMS_Solution_Id,
                        principalTable: "CMS_Solution",
                        principalColumn: "CMS_Solution_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Article_Technology",
                columns: table => new
                {
                    CMS_Article_Technology_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMS_Article_Id = table.Column<int>(type: "int", nullable: false),
                    CMS_Technology_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Article_Technology", x => x.CMS_Article_Technology_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Technology_CMS_Article_CMS_Article_Id",
                        column: x => x.CMS_Article_Id,
                        principalTable: "CMS_Article",
                        principalColumn: "CMS_Article_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Technology_CMS_Technology_CMS_Technology_Id",
                        column: x => x.CMS_Technology_Id,
                        principalTable: "CMS_Technology",
                        principalColumn: "CMS_Technology_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CMS_Article_Type",
                columns: new[] { "Article_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Article_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 13, null, null, null, "Video", null },
                    { 14, null, null, null, "Audio", null },
                    { 15, null, null, null, "PDF", null },
                    { 16, null, null, null, "Market Brief", null },
                    { 17, null, null, null, "Articles", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Type_Id",
                table: "CMS_Article",
                column: "Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Category_CMS_Article_Id",
                table: "CMS_Article_Category",
                column: "CMS_Article_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Category_CMS_Category_Id",
                table: "CMS_Article_Category",
                column: "CMS_Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Region_CMS_Article_Id",
                table: "CMS_Article_Region",
                column: "CMS_Article_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Region_CMS_Region_Id",
                table: "CMS_Article_Region",
                column: "CMS_Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Solution_CMS_Article_Id",
                table: "CMS_Article_Solution",
                column: "CMS_Article_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Solution_CMS_Solution_Id",
                table: "CMS_Article_Solution",
                column: "CMS_Solution_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Technology_CMS_Article_Id",
                table: "CMS_Article_Technology",
                column: "CMS_Article_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Technology_CMS_Technology_Id",
                table: "CMS_Article_Technology",
                column: "CMS_Technology_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CMS_Article_Category");

            migrationBuilder.DropTable(
                name: "CMS_Article_Region");

            migrationBuilder.DropTable(
                name: "CMS_Article_Solution");

            migrationBuilder.DropTable(
                name: "CMS_Article_Technology");

            migrationBuilder.DropTable(
                name: "CMS_Article");

            migrationBuilder.DropTable(
                name: "CMS_Article_Type");
        }
    }
}
