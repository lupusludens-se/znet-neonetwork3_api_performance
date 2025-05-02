using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddPermissionsPart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Permission_Id = table.Column<int>(type: "int", nullable: false),
                    Permission_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_Ts = table.Column<int>(type: "datetime2", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Last_Change_Ts = table.Column<int>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Permission_Id);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Permission_Name" },
                values: new object[,]
                {
                    { 1, "Account settings edit my own" },
                    { 2, "Account settings edit others" },
                    { 3, "Announcement add to dashboard" },
                    { 4, "Comment delete my own" },
                    { 5, "Comment delete others" },
                    { 6, "Comment pin" },
                    { 7, "Company add" },
                    { 8, "Event create" },
                    { 9, "Event delete" },
                    { 10, "Event edit" },
                    { 11, "Event save draft" },
                    { 12, "Project add" },
                    { 13, "Project delete published by me" },
                    { 14, "Project delete published by others" },
                    { 15, "Project edit published by me" },
                    { 16, "Project edit published by others" },
                    { 17, "Project save draft" },
                    { 18, "Project mark as favorite" },
                    { 19, "Project view all" },
                    { 20, "Provider contact" },
                    { 21, "Tool add" },
                    { 22, "Tool delete" },
                    { 23, "Tool edit" },
                    { 24, "Tool view" },
                    { 25, "Tool view all" },
                    { 26, "Topic add" },
                    { 27, "Topic create private" },
                    { 28, "Topic delete published by others" },
                    { 29, "Topic edit published by others" },
                    { 30, "Topic pin" },
                    { 31, "User add" }
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
