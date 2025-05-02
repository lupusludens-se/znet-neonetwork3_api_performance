using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class removeIndexesForIniative : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Initiative_Tool_Initiative_Id_Tool_Id",
                table: "Initiative_Tool");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Region_Initiative_Id_CMS_Region_Id",
                table: "Initiative_Region");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Project_Initiative_Id_Project_Id",
                table: "Initiative_Project");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Discussion_Initiative_Id_Discussion_Id",
                table: "Initiative_Discussion");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Community_Initiative_Id_User_Id",
                table: "Initiative_Community");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Article_Initiative_Id_Article_Id",
                table: "Initiative_Article");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Activity_Initiative_Id_User_Id",
                table: "Initiative_Activity");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Tool_Initiative_Id",
                table: "Initiative_Tool",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Region_Initiative_Id",
                table: "Initiative_Region",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Project_Initiative_Id",
                table: "Initiative_Project",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Discussion_Initiative_Id",
                table: "Initiative_Discussion",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Community_Initiative_Id",
                table: "Initiative_Community",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Article_Initiative_Id",
                table: "Initiative_Article",
                column: "Initiative_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Activity_Initiative_Id",
                table: "Initiative_Activity",
                column: "Initiative_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Initiative_Tool_Initiative_Id",
                table: "Initiative_Tool");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Region_Initiative_Id",
                table: "Initiative_Region");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Project_Initiative_Id",
                table: "Initiative_Project");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Discussion_Initiative_Id",
                table: "Initiative_Discussion");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Community_Initiative_Id",
                table: "Initiative_Community");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Article_Initiative_Id",
                table: "Initiative_Article");

            migrationBuilder.DropIndex(
                name: "IX_Initiative_Activity_Initiative_Id",
                table: "Initiative_Activity");

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Tool_Initiative_Id_Tool_Id",
                table: "Initiative_Tool",
                columns: new[] { "Initiative_Id", "Tool_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Region_Initiative_Id_CMS_Region_Id",
                table: "Initiative_Region",
                columns: new[] { "Initiative_Id", "CMS_Region_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Project_Initiative_Id_Project_Id",
                table: "Initiative_Project",
                columns: new[] { "Initiative_Id", "Project_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Discussion_Initiative_Id_Discussion_Id",
                table: "Initiative_Discussion",
                columns: new[] { "Initiative_Id", "Discussion_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Community_Initiative_Id_User_Id",
                table: "Initiative_Community",
                columns: new[] { "Initiative_Id", "User_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Article_Initiative_Id_Article_Id",
                table: "Initiative_Article",
                columns: new[] { "Initiative_Id", "Article_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Initiative_Activity_Initiative_Id_User_Id",
                table: "Initiative_Activity",
                columns: new[] { "Initiative_Id", "User_Id" },
                unique: true);
        }
    }
}
