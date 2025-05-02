using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class setuseronetoonezerouserprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Category_User_Profile_User_Profile_Id",
                table: "User_Profile_Category");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Region_User_Profile_User_Profile_Id",
                table: "User_Profile_Region");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Solution_User_Profile_User_Profile_Id",
                table: "User_Profile_Solution");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Technology_User_Profile_User_Profile_Id",
                table: "User_Profile_Technology");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Url_Link_User_Profile_User_Profile_Id",
                table: "User_Profile_Url_Link");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Profile",
                table: "User_Profile");

            migrationBuilder.DropIndex(
                name: "IX_User_Profile_User_Id",
                table: "User_Profile");

            migrationBuilder.DropColumn(
                name: "User_Profile_Id",
                table: "User_Profile");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Profile",
                table: "User_Profile",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Category_User_Profile_User_Profile_Id",
                table: "User_Profile_Category",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Region_User_Profile_User_Profile_Id",
                table: "User_Profile_Region",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Solution_User_Profile_User_Profile_Id",
                table: "User_Profile_Solution",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Technology_User_Profile_User_Profile_Id",
                table: "User_Profile_Technology",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Url_Link_User_Profile_User_Profile_Id",
                table: "User_Profile_Url_Link",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Category_User_Profile_User_Profile_Id",
                table: "User_Profile_Category");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Region_User_Profile_User_Profile_Id",
                table: "User_Profile_Region");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Solution_User_Profile_User_Profile_Id",
                table: "User_Profile_Solution");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Technology_User_Profile_User_Profile_Id",
                table: "User_Profile_Technology");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Profile_Url_Link_User_Profile_User_Profile_Id",
                table: "User_Profile_Url_Link");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Profile",
                table: "User_Profile");

            migrationBuilder.AddColumn<int>(
                name: "User_Profile_Id",
                table: "User_Profile",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Profile",
                table: "User_Profile",
                column: "User_Profile_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profile_User_Id",
                table: "User_Profile",
                column: "User_Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Category_User_Profile_User_Profile_Id",
                table: "User_Profile_Category",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Profile_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Region_User_Profile_User_Profile_Id",
                table: "User_Profile_Region",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Profile_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Solution_User_Profile_User_Profile_Id",
                table: "User_Profile_Solution",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Profile_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Technology_User_Profile_User_Profile_Id",
                table: "User_Profile_Technology",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Profile_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Profile_Url_Link_User_Profile_User_Profile_Id",
                table: "User_Profile_Url_Link",
                column: "User_Profile_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Profile_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
