using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class ExtendedProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 34);

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "Project",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Project_Name",
                table: "Project",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<int>(
                name: "Category_Id",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Owner_Id",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedOn",
                table: "Project",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status_Id",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Project_Region",
                columns: table => new
                {
                    Project_Region_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Region_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Region", x => x.Project_Region_Id);
                    table.ForeignKey(
                        name: "FK_Project_Region_CMS_Region_Region_Id",
                        column: x => x.Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Region_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Status",
                columns: table => new
                {
                    Project_Status_Id = table.Column<int>(type: "int", nullable: false),
                    Project_Status_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Status", x => x.Project_Status_Id);
                });

            migrationBuilder.CreateTable(
                name: "Project_Technology",
                columns: table => new
                {
                    Project_Technology_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Technology_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Technology", x => x.Project_Technology_Id);
                    table.ForeignKey(
                        name: "FK_Project_Technology_CMS_Technology_Technology_Id",
                        column: x => x.Technology_Id,
                        principalTable: "CMS_Technology",
                        principalColumn: "CMS_Technology_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Technology_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Project edit published by me");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Project edit published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "Project save draft");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "Project mark as favorite");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "Project view all");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 16,
                column: "Permission_Name",
                value: "Provider contact");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 17,
                column: "Permission_Name",
                value: "Tool add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 18,
                column: "Permission_Name",
                value: "Tool delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 19,
                column: "Permission_Name",
                value: "Tool edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 20,
                column: "Permission_Name",
                value: "Tool view all");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 21,
                column: "Permission_Name",
                value: "Topic create private");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 22,
                column: "Permission_Name",
                value: "Topic delete published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 23,
                column: "Permission_Name",
                value: "Topic edit published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 24,
                column: "Permission_Name",
                value: "Topic pin");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 25,
                column: "Permission_Name",
                value: "User add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 26,
                column: "Permission_Name",
                value: "Topic view private");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 27,
                column: "Permission_Name",
                value: "User export");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 28,
                column: "Permission_Name",
                value: "User edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 29,
                column: "Permission_Name",
                value: "User admit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 30,
                column: "Permission_Name",
                value: "Conversation view all");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 31,
                column: "Permission_Name",
                value: "Conversation create group");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 32,
                column: "Permission_Name",
                value: "Event view all");

            migrationBuilder.InsertData(
                table: "Project_Status",
                columns: new[] { "Project_Status_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Project_Status_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "Active", null },
                    { 2, null, null, null, "Inactive", null },
                    { 3, null, null, null, "Draft", null },
                    { 4, null, null, null, "Deleted", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Category_Id",
                table: "Project",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Owner_Id",
                table: "Project",
                column: "Owner_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Status_Id",
                table: "Project",
                column: "Status_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Region_Project_Id",
                table: "Project_Region",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Region_Region_Id",
                table: "Project_Region",
                column: "Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Technology_Project_Id",
                table: "Project_Technology",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Technology_Technology_Id",
                table: "Project_Technology",
                column: "Technology_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_CMS_Category_Category_Id",
                table: "Project",
                column: "Category_Id",
                principalTable: "CMS_Category",
                principalColumn: "CMS_Category_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Project_Status_Status_Id",
                table: "Project",
                column: "Status_Id",
                principalTable: "Project_Status",
                principalColumn: "Project_Status_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_Owner_Id",
                table: "Project",
                column: "Owner_Id",
                principalTable: "User",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_CMS_Category_Category_Id",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Project_Status_Status_Id",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_User_Owner_Id",
                table: "Project");

            migrationBuilder.DropTable(
                name: "Project_Region");

            migrationBuilder.DropTable(
                name: "Project_Status");

            migrationBuilder.DropTable(
                name: "Project_Technology");

            migrationBuilder.DropIndex(
                name: "IX_Project_Category_Id",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_Owner_Id",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_Status_Id",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Category_Id",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Owner_Id",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "PublishedOn",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Status_Id",
                table: "Project");

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Project_Name",
                table: "Project",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Project delete published by me");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Project delete published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "Project edit published by me");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "Project edit published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "Project save draft");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 16,
                column: "Permission_Name",
                value: "Project mark as favorite");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 17,
                column: "Permission_Name",
                value: "Project view all");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 18,
                column: "Permission_Name",
                value: "Provider contact");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 19,
                column: "Permission_Name",
                value: "Tool add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 20,
                column: "Permission_Name",
                value: "Tool delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 21,
                column: "Permission_Name",
                value: "Tool edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 22,
                column: "Permission_Name",
                value: "Tool view all");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 23,
                column: "Permission_Name",
                value: "Topic create private");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 24,
                column: "Permission_Name",
                value: "Topic delete published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 25,
                column: "Permission_Name",
                value: "Topic edit published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 26,
                column: "Permission_Name",
                value: "Topic pin");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 27,
                column: "Permission_Name",
                value: "User add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 28,
                column: "Permission_Name",
                value: "Topic view private");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 29,
                column: "Permission_Name",
                value: "User export");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 30,
                column: "Permission_Name",
                value: "User edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 31,
                column: "Permission_Name",
                value: "User admit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 32,
                column: "Permission_Name",
                value: "Conversation view all");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 33, null, null, null, "Conversation create group", null },
                    { 34, null, null, null, "Event view all", null }
                });
        }
    }
}
