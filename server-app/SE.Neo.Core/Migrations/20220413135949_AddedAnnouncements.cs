using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedAnnouncements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcement",
                columns: table => new
                {
                    Announcement_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Background_Image_Name = table.Column<string>(type: "nvarchar(1024)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Button_Text = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Button_Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Audience_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcement", x => x.Announcement_Id);
                    table.ForeignKey(
                        name: "FK_Announcement_Blob_Background_Image_Name",
                        column: x => x.Background_Image_Name,
                        principalTable: "Blob",
                        principalColumn: "Blob_Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Announcement_Role_Audience_Id",
                        column: x => x.Audience_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Blob_Container",
                columns: new[] { "Blob_Container_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Blob_Container_Name", "Updated_User_Id" },
                values: new object[] { 6, null, null, null, "Announcement", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 2,
                column: "Permission_Name",
                value: "Announcement manage");

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_Audience_Id",
                table: "Announcement",
                column: "Audience_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_Background_Image_Name",
                table: "Announcement",
                column: "Background_Image_Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcement");

            migrationBuilder.DeleteData(
                table: "Blob_Container",
                keyColumn: "Blob_Container_Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 2,
                column: "Permission_Name",
                value: "Announcement add to dashboard");
        }
    }
}
