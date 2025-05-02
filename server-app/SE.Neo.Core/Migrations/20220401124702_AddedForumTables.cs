using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedForumTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_MessageId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_MessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "Message_Pinned",
                table: "Message",
                newName: "Message_Is_Pinned");

            migrationBuilder.RenameColumn(
                name: "Discussion_Pinned",
                table: "Discussion",
                newName: "Discussion_Is_Pinned");

            migrationBuilder.AlterColumn<string>(
                name: "Message_Text",
                table: "Message",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Discussion_Is_Deleted",
                table: "Discussion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Message_Id",
                table: "Attachment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image_Name",
                table: "Attachment",
                type: "nvarchar(1024)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Discussion_Category",
                columns: table => new
                {
                    Discussion_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Disscussion_Id = table.Column<int>(type: "int", nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion_Category", x => x.Discussion_Category_Id);
                    table.ForeignKey(
                        name: "FK_Discussion_Category_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Discussion_Category_Discussion_Disscussion_Id",
                        column: x => x.Disscussion_Id,
                        principalTable: "Discussion",
                        principalColumn: "Discussion_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Discussion_Follower",
                columns: table => new
                {
                    Discussion_Follower_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discussion_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion_Follower", x => x.Discussion_Follower_Id);
                    table.ForeignKey(
                        name: "FK_Discussion_Follower_Discussion_Discussion_Id",
                        column: x => x.Discussion_Id,
                        principalTable: "Discussion",
                        principalColumn: "Discussion_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Discussion_Follower_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Discussion_Region",
                columns: table => new
                {
                    Discussion_Region_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Disscussion_Id = table.Column<int>(type: "int", nullable: false),
                    Region_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion_Region", x => x.Discussion_Region_Id);
                    table.ForeignKey(
                        name: "FK_Discussion_Region_CMS_Region_Region_Id",
                        column: x => x.Region_Id,
                        principalTable: "CMS_Region",
                        principalColumn: "CMS_Region_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Discussion_Region_Discussion_Disscussion_Id",
                        column: x => x.Disscussion_Id,
                        principalTable: "Discussion",
                        principalColumn: "Discussion_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Discussion_Saved",
                columns: table => new
                {
                    Discussion_Saved_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discussion_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    SavedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion_Saved", x => x.Discussion_Saved_Id);
                    table.ForeignKey(
                        name: "FK_Discussion_Saved_Discussion_Discussion_Id",
                        column: x => x.Discussion_Id,
                        principalTable: "Discussion",
                        principalColumn: "Discussion_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Discussion_Saved_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message_Like",
                columns: table => new
                {
                    Message_Like_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Message_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message_Like", x => x.Message_Like_Id);
                    table.ForeignKey(
                        name: "FK_Message_Like_Message_Message_Id",
                        column: x => x.Message_Id,
                        principalTable: "Message",
                        principalColumn: "Message_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Like_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Blob_Container",
                columns: new[] { "Blob_Container_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Blob_Container_Name", "Updated_User_Id" },
                values: new object[] { 4, null, null, null, "Forums", null });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[] { 28, null, null, null, "Topic view private", null });

            migrationBuilder.CreateIndex(
                name: "IX_Message_Parent_Message_Id",
                table: "Message",
                column: "Parent_Message_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_Image_Name",
                table: "Attachment",
                column: "Image_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Category_Category_Id",
                table: "Discussion_Category",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Category_Disscussion_Id_Category_Id",
                table: "Discussion_Category",
                columns: new[] { "Disscussion_Id", "Category_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Follower_Discussion_Id_User_Id",
                table: "Discussion_Follower",
                columns: new[] { "Discussion_Id", "User_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Follower_User_Id",
                table: "Discussion_Follower",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Region_Disscussion_Id_Region_Id",
                table: "Discussion_Region",
                columns: new[] { "Disscussion_Id", "Region_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Region_Region_Id",
                table: "Discussion_Region",
                column: "Region_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Saved_Discussion_Id_User_Id",
                table: "Discussion_Saved",
                columns: new[] { "Discussion_Id", "User_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Saved_User_Id",
                table: "Discussion_Saved",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Like_Message_Id",
                table: "Message_Like",
                column: "Message_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Like_User_Id",
                table: "Message_Like",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Blob_Image_Name",
                table: "Attachment",
                column: "Image_Name",
                principalTable: "Blob",
                principalColumn: "Blob_Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_Parent_Message_Id",
                table: "Message",
                column: "Parent_Message_Id",
                principalTable: "Message",
                principalColumn: "Message_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Blob_Image_Name",
                table: "Attachment");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_Parent_Message_Id",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Discussion_Category");

            migrationBuilder.DropTable(
                name: "Discussion_Follower");

            migrationBuilder.DropTable(
                name: "Discussion_Region");

            migrationBuilder.DropTable(
                name: "Discussion_Saved");

            migrationBuilder.DropTable(
                name: "Message_Like");

            migrationBuilder.DropIndex(
                name: "IX_Message_Parent_Message_Id",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Attachment_Image_Name",
                table: "Attachment");

            migrationBuilder.DeleteData(
                table: "Blob_Container",
                keyColumn: "Blob_Container_Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 28);

            migrationBuilder.DropColumn(
                name: "Discussion_Is_Deleted",
                table: "Discussion");

            migrationBuilder.DropColumn(
                name: "Image_Name",
                table: "Attachment");

            migrationBuilder.RenameColumn(
                name: "Message_Is_Pinned",
                table: "Message",
                newName: "Message_Pinned");

            migrationBuilder.RenameColumn(
                name: "Discussion_Is_Pinned",
                table: "Discussion",
                newName: "Discussion_Pinned");

            migrationBuilder.AlterColumn<string>(
                name: "Message_Text",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Message_Id",
                table: "Attachment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Message_MessageId",
                table: "Message",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_MessageId",
                table: "Message",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Message_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
