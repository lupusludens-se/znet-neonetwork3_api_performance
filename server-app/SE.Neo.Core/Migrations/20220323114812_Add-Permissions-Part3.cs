using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddPermissionsPart3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 31);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 1,
                column: "Permission_Name",
                value: "Account settings edit others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 2,
                column: "Permission_Name",
                value: "Announcement add to dashboard");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 3,
                column: "Permission_Name",
                value: "Comment delete others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 4,
                column: "Permission_Name",
                value: "Comment pin");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Company add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 6,
                column: "Permission_Name",
                value: "Event create");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 7,
                column: "Permission_Name",
                value: "Event delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 8,
                column: "Permission_Name",
                value: "Event edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 9,
                column: "Permission_Name",
                value: "Event save draft");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 10,
                column: "Permission_Name",
                value: "Project add");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 1,
                column: "Permission_Name",
                value: "Account settings edit my own");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 2,
                column: "Permission_Name",
                value: "Account settings edit others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 3,
                column: "Permission_Name",
                value: "Announcement add to dashboard");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 4,
                column: "Permission_Name",
                value: "Comment delete my own");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Comment delete others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 6,
                column: "Permission_Name",
                value: "Comment pin");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 7,
                column: "Permission_Name",
                value: "Company add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 8,
                column: "Permission_Name",
                value: "Event create");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 9,
                column: "Permission_Name",
                value: "Event delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 10,
                column: "Permission_Name",
                value: "Event edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Event save draft");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Project add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "Project delete published by me");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "Project delete published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "Project edit published by me");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 16,
                column: "Permission_Name",
                value: "Project edit published by others");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 17,
                column: "Permission_Name",
                value: "Project save draft");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 18,
                column: "Permission_Name",
                value: "Project mark as favorite");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 19,
                column: "Permission_Name",
                value: "Project view all");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 20,
                column: "Permission_Name",
                value: "Provider contact");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 21,
                column: "Permission_Name",
                value: "Tool add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 22,
                column: "Permission_Name",
                value: "Tool delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 23,
                column: "Permission_Name",
                value: "Tool edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 24,
                column: "Permission_Name",
                value: "Tool view");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 25,
                column: "Permission_Name",
                value: "Tool view all");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 26,
                column: "Permission_Name",
                value: "Topic add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 27,
                column: "Permission_Name",
                value: "Topic create private");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Permission_Name" },
                values: new object[,]
                {
                    { 28, "Topic delete published by others" },
                    { 29, "Topic edit published by others" },
                    { 30, "Topic pin" },
                    { 31, "User add" }
                });
        }
    }
}
