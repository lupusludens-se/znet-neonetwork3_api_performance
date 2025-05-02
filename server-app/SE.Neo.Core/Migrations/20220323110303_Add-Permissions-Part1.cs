using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddPermissionsPart1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Permission_Permission_Permission_Id",
                table: "Role_Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Permission_Permission_Permission_Id",
                table: "User_Permission");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_User_Permission_Permission_Id",
                table: "User_Permission");

            migrationBuilder.DropIndex(
                name: "IX_Role_Permission_Permission_Id",
                table: "Role_Permission");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Permission_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Permission_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Permission_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Permission_Permission_Id",
                table: "User_Permission",
                column: "Permission_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Permission_Permission_Id",
                table: "Role_Permission",
                column: "Permission_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Permission_Permission_Permission_Id",
                table: "Role_Permission",
                column: "Permission_Id",
                principalTable: "Permission",
                principalColumn: "Permission_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Permission_Permission_Permission_Id",
                table: "User_Permission",
                column: "Permission_Id",
                principalTable: "Permission",
                principalColumn: "Permission_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
