using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class modifycompanyentityindustry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Location_Default_Location_Id",
                table: "Company");

            migrationBuilder.DropTable(
                name: "Company_Location");

            migrationBuilder.DropIndex(
                name: "IX_Company_Default_Location_Id",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Contracts",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Default_Location_Id",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Installed_Location",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Is_EAC_Included",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Payback_Period",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Technology",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Term_Length",
                table: "Company");

            migrationBuilder.RenameColumn(
                name: "User_Profile_Solution_Id",
                table: "User_Profile_Category",
                newName: "User_Profile_Category_Id");

            migrationBuilder.RenameColumn(
                name: "Web_Site_Link",
                table: "Company",
                newName: "About");

            migrationBuilder.AlterColumn<int>(
                name: "Country_Id",
                table: "User",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Company_Url",
                table: "Company",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image_Logo",
                table: "Company",
                type: "nvarchar(1024)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn_Url",
                table: "Company",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role_Id",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "Status_Id",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "Company_Category",
                columns: table => new
                {
                    Company_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Category", x => x.Company_Category_Id);
                    table.ForeignKey(
                        name: "FK_Company_Category_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Category_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Company_Status",
                columns: table => new
                {
                    Company_Status_Id = table.Column<int>(type: "int", nullable: false),
                    Company_Status_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Status", x => x.Company_Status_Id);
                });

            migrationBuilder.CreateTable(
                name: "Company_Url_Link",
                columns: table => new
                {
                    Url_Link_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url_Link = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Url_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Url_Link", x => x.Url_Link_Id);
                    table.ForeignKey(
                        name: "FK_Company_Url_Link_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Industry",
                columns: table => new
                {
                    Industry_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Industry_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industry", x => x.Industry_Id);
                });

            migrationBuilder.CreateTable(
                name: "Project_Location",
                columns: table => new
                {
                    Project_Location_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Id = table.Column<int>(type: "int", nullable: false),
                    Location_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Location", x => x.Project_Location_Id);
                    table.ForeignKey(
                        name: "FK_Project_Location_Location_Location_Id",
                        column: x => x.Location_Id,
                        principalTable: "Location",
                        principalColumn: "Location_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Location_Project_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Project",
                        principalColumn: "Project_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Company_Status",
                columns: new[] { "Company_Status_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Company_Status_Name", "Updated_User_Id" },
                values: new object[] { 1, null, null, null, "Active", null });

            migrationBuilder.InsertData(
                table: "Company_Status",
                columns: new[] { "Company_Status_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Company_Status_Name", "Updated_User_Id" },
                values: new object[] { 2, null, null, null, "Inactive", null });

            migrationBuilder.InsertData(
                table: "Company_Status",
                columns: new[] { "Company_Status_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Company_Status_Name", "Updated_User_Id" },
                values: new object[] { 3, null, null, null, "Deleted", null });

            migrationBuilder.CreateIndex(
                name: "IX_Company_Image_Logo",
                table: "Company",
                column: "Image_Logo");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Status_Id",
                table: "Company",
                column: "Status_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Category_Category_Id",
                table: "Company_Category",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Category_Company_Id",
                table: "Company_Category",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Url_Link_Company_Id",
                table: "Company_Url_Link",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Location_Location_Id",
                table: "Project_Location",
                column: "Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Location_Project_Id",
                table: "Project_Location",
                column: "Project_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Blob_Image_Logo",
                table: "Company",
                column: "Image_Logo",
                principalTable: "Blob",
                principalColumn: "Blob_Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Company_Status_Status_Id",
                table: "Company",
                column: "Status_Id",
                principalTable: "Company_Status",
                principalColumn: "Company_Status_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.InsertData(
                table: "Industry",
                columns: new[] { "Industry_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Industry_Name", "Updated_User_Id" },
                values: new object[] { 1, null, null, null, "Retail", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Blob_Image_Logo",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Company_Status_Status_Id",
                table: "Company");

            migrationBuilder.DropTable(
                name: "Company_Category");

            migrationBuilder.DropTable(
                name: "Company_Status");

            migrationBuilder.DropTable(
                name: "Company_Url_Link");

            migrationBuilder.DropTable(
                name: "Industry");

            migrationBuilder.DropTable(
                name: "Project_Location");

            migrationBuilder.DropIndex(
                name: "IX_Company_Image_Logo",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_Status_Id",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Company_Url",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Image_Logo",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LinkedIn_Url",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Role_Id",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Status_Id",
                table: "Company");

            migrationBuilder.RenameColumn(
                name: "User_Profile_Category_Id",
                table: "User_Profile_Category",
                newName: "User_Profile_Solution_Id");

            migrationBuilder.RenameColumn(
                name: "About",
                table: "Company",
                newName: "Web_Site_Link");

            migrationBuilder.AlterColumn<int>(
                name: "Country_Id",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Contracts",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Default_Location_Id",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Installed_Location",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Is_EAC_Included",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Payback_Period",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Technology",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Term_Length",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Company_Location",
                columns: table => new
                {
                    Company_Location_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_Id = table.Column<int>(type: "int", nullable: false),
                    Location_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Location", x => x.Company_Location_Id);
                    table.ForeignKey(
                        name: "FK_Company_Location_Company_Company_Id",
                        column: x => x.Company_Id,
                        principalTable: "Company",
                        principalColumn: "Company_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Location_Location_Location_Id",
                        column: x => x.Location_Id,
                        principalTable: "Location",
                        principalColumn: "Location_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_Default_Location_Id",
                table: "Company",
                column: "Default_Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Location_Company_Id",
                table: "Company_Location",
                column: "Company_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Location_Location_Id",
                table: "Company_Location",
                column: "Location_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Location_Default_Location_Id",
                table: "Company",
                column: "Default_Location_Id",
                principalTable: "Location",
                principalColumn: "Location_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
