using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class add_article_role_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CMS_Article_Role",
                columns: table => new
                {
                    CMS_Article_Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMS_Article_Id = table.Column<int>(type: "int", nullable: false),
                    CMS_Role_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Article_Role", x => x.CMS_Article_Role_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Role_CMS_Article_CMS_Article_Id",
                        column: x => x.CMS_Article_Id,
                        principalTable: "CMS_Article",
                        principalColumn: "CMS_Article_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Role_Role_CMS_Role_Id",
                        column: x => x.CMS_Role_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Role_CMS_Article_Id",
                table: "CMS_Article_Role",
                column: "CMS_Article_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Role_CMS_Role_Id",
                table: "CMS_Article_Role",
                column: "CMS_Role_Id");

            migrationBuilder.Sql("UPDATE [dbo].[CMS_Article] SET [Modified_Date] = '2021-05-06'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CMS_Article_Role");
        }
    }
}
