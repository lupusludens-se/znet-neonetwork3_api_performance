using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class createtablecontenttagsandcreatetablearticlecontenttags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CMS_Article_Content_Tag",
                columns: table => new
                {
                    CMS_Article_Content_Tag_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMS_Article_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Article_Content_Tag", x => x.CMS_Article_Content_Tag_Id);
                    table.ForeignKey(
                        name: "FK_CMS_Article_Content_Tag_CMS_Article_CMS_Article_Id",
                        column: x => x.CMS_Article_Id,
                        principalTable: "CMS_Article",
                        principalColumn: "CMS_Article_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Content_Tag",
                columns: table => new
                {
                    CMS_Content_Tag_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content_Tag_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Content_Tag_Slug = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Is_Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Content_Tag", x => x.CMS_Content_Tag_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMS_Article_Content_Tag_CMS_Article_Id",
                table: "CMS_Article_Content_Tag",
                column: "CMS_Article_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CMS_Article_Content_Tag");

            migrationBuilder.DropTable(
                name: "CMS_Content_Tag");
        }
    }
}
