using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class ReplaceConversationRelatedTablesWithDiscussionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Conversation_Conversation_Id",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_Profile_User_From_Id",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_Profile_User_To_Id",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Message_Conversation_Id",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Conversation_Id",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "User_To_Id",
                table: "Message",
                newName: "User_Id");

            migrationBuilder.RenameColumn(
                name: "User_From_Id",
                table: "Message",
                newName: "Discussion_Id");

            migrationBuilder.RenameColumn(
                name: "Is_Read",
                table: "Message",
                newName: "Message_Pinned");

            migrationBuilder.RenameIndex(
                name: "IX_Message_User_To_Id",
                table: "Message",
                newName: "IX_Message_User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Message_User_From_Id",
                table: "Message",
                newName: "IX_Message_Discussion_Id");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Parent_Message_Id",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Discussion",
                columns: table => new
                {
                    Discussion_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discussion_Subject = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Discussion_Type = table.Column<int>(type: "int", nullable: false),
                    Discussion_Pinned = table.Column<bool>(type: "bit", nullable: false),
                    Project_Id = table.Column<int>(type: "int", nullable: true),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion", x => x.Discussion_Id);
                    table.ForeignKey(
                        name: "FK_Discussion_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Discussion_User",
                columns: table => new
                {
                    Discussion_User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Discussion_Id = table.Column<int>(type: "int", nullable: false),
                    UnreadCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion_User", x => x.Discussion_User_Id);
                    table.ForeignKey(
                        name: "FK_Discussion_User_Discussion_Discussion_Id",
                        column: x => x.Discussion_Id,
                        principalTable: "Discussion",
                        principalColumn: "Discussion_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Discussion_User_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_MessageId",
                table: "Message",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_Project_Id",
                table: "Discussion",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_User_Discussion_Id",
                table: "Discussion_User",
                column: "Discussion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_User_User_Id",
                table: "Discussion_User",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Discussion_Discussion_Id",
                table: "Message",
                column: "Discussion_Id",
                principalTable: "Discussion",
                principalColumn: "Discussion_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_MessageId",
                table: "Message",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Message_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_User_Id",
                table: "Message",
                column: "User_Id",
                principalTable: "User",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Discussion_Discussion_Id",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_MessageId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_User_Id",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Discussion_User");

            migrationBuilder.DropTable(
                name: "Discussion");

            migrationBuilder.DropIndex(
                name: "IX_Message_MessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Parent_Message_Id",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "Message",
                newName: "User_To_Id");

            migrationBuilder.RenameColumn(
                name: "Message_Pinned",
                table: "Message",
                newName: "Is_Read");

            migrationBuilder.RenameColumn(
                name: "Discussion_Id",
                table: "Message",
                newName: "User_From_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Message_User_Id",
                table: "Message",
                newName: "IX_Message_User_To_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Message_Discussion_Id",
                table: "Message",
                newName: "IX_Message_User_From_Id");

            migrationBuilder.AddColumn<int>(
                name: "Conversation_Id",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Message",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Conversation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Bcc_Id = table.Column<int>(type: "int", nullable: true),
                    User_From_Id = table.Column<int>(type: "int", nullable: false),
                    User_To_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conversation_Type = table.Column<int>(type: "int", nullable: false),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Conversation_Id);
                    table.ForeignKey(
                        name: "FK_Conversation_User_Profile_User_Bcc_Id",
                        column: x => x.User_Bcc_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_User_Profile_User_From_Id",
                        column: x => x.User_From_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_User_Profile_User_To_Id",
                        column: x => x.User_To_Id,
                        principalTable: "User_Profile",
                        principalColumn: "User_Profile_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_Conversation_Id",
                table: "Message",
                column: "Conversation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_User_Bcc_Id",
                table: "Conversation",
                column: "User_Bcc_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_User_From_Id",
                table: "Conversation",
                column: "User_From_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_User_To_Id",
                table: "Conversation",
                column: "User_To_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Conversation_Conversation_Id",
                table: "Message",
                column: "Conversation_Id",
                principalTable: "Conversation",
                principalColumn: "Conversation_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_Profile_User_From_Id",
                table: "Message",
                column: "User_From_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Profile_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_Profile_User_To_Id",
                table: "Message",
                column: "User_To_Id",
                principalTable: "User_Profile",
                principalColumn: "User_Profile_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
