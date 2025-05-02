using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedBlob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image_Key",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Image_Name",
                table: "User",
                type: "nvarchar(1024)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blob_Container",
                columns: table => new
                {
                    Blob_Container_Id = table.Column<int>(type: "int", nullable: false),
                    Blob_Container_Name = table.Column<string>(type: "nvarchar(63)", maxLength: 63, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blob_Container", x => x.Blob_Container_Id);
                });

            migrationBuilder.CreateTable(
                name: "Blob",
                columns: table => new
                {
                    Blob_Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Blob_Container_Name = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blob", x => x.Blob_Name);
                    table.ForeignKey(
                        name: "FK_Blob_Blob_Container_Blob_Container_Name",
                        column: x => x.Blob_Container_Name,
                        principalTable: "Blob_Container",
                        principalColumn: "Blob_Container_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Blob_Container",
                columns: new[] { "Blob_Container_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Blob_Container_Name", "Updated_User_Id" },
                values: new object[] { 1, null, null, null, "Users", null });

            migrationBuilder.InsertData(
                table: "Blob_Container",
                columns: new[] { "Blob_Container_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Blob_Container_Name", "Updated_User_Id" },
                values: new object[] { 2, null, null, null, "Conversations", null });

            migrationBuilder.CreateIndex(
                name: "IX_User_Image_Name",
                table: "User",
                column: "Image_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Blob_Blob_Container_Name",
                table: "Blob",
                column: "Blob_Container_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Blob_Image_Name",
                table: "User",
                column: "Image_Name",
                principalTable: "Blob",
                principalColumn: "Blob_Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Blob_Image_Name",
                table: "User");

            migrationBuilder.DropTable(
                name: "Blob");

            migrationBuilder.DropTable(
                name: "Blob_Container");

            migrationBuilder.DropIndex(
                name: "IX_User_Image_Name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Image_Name",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Image_Key",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
