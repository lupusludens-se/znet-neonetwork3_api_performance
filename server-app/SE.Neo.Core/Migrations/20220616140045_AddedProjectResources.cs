using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SE.Neo.Core.Migrations
{
    public partial class AddedProjectResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 16);

            migrationBuilder.CreateTable(
                name: "Resource_Type",
                columns: table => new
                {
                    Resource_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Resource_Type_Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource_Type", x => x.Resource_Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Resource_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content_Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Reference_Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Type_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Resource_Id);
                    table.ForeignKey(
                        name: "FK_Resource_Resource_Type_Type_Id",
                        column: x => x.Type_Id,
                        principalTable: "Resource_Type",
                        principalColumn: "Resource_Type_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resource_Category",
                columns: table => new
                {
                    Resource_Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resource_Id = table.Column<int>(type: "int", nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource_Category", x => x.Resource_Category_Id);
                    table.ForeignKey(
                        name: "FK_Resource_Category_CMS_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "CMS_Category",
                        principalColumn: "CMS_Category_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resource_Category_Resource_Resource_Id",
                        column: x => x.Resource_Id,
                        principalTable: "Resource",
                        principalColumn: "Resource_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resource_Technology",
                columns: table => new
                {
                    Project_Technology_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resource_Id = table.Column<int>(type: "int", nullable: false),
                    Technology_Id = table.Column<int>(type: "int", nullable: false),
                    Created_User_Id = table.Column<int>(type: "int", nullable: true),
                    Updated_User_Id = table.Column<int>(type: "int", nullable: true),
                    Created_Ts = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Last_Change_Ts = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource_Technology", x => x.Project_Technology_Id);
                    table.ForeignKey(
                        name: "FK_Resource_Technology_CMS_Technology_Technology_Id",
                        column: x => x.Technology_Id,
                        principalTable: "CMS_Technology",
                        principalColumn: "CMS_Technology_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resource_Technology_Resource_Resource_Id",
                        column: x => x.Resource_Id,
                        principalTable: "Resource",
                        principalColumn: "Resource_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 4,
                column: "Permission_Name",
                value: "Data Sync");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Event Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 6,
                column: "Permission_Name",
                value: "Messages All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 7,
                column: "Permission_Name",
                value: "Project Catalog View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 8,
                column: "Permission_Name",
                value: "Project Management All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 9,
                column: "Permission_Name",
                value: "Project Management Own");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 10,
                column: "Permission_Name",
                value: "Send Quote");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Tool Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Forum Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "User Access Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "User Profile Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "Email Alert Management");

            migrationBuilder.InsertData(
                table: "Resource_Type",
                columns: new[] { "Resource_Type_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Resource_Type_Name", "Updated_User_Id" },
                values: new object[,]
                {
                    { 1, null, null, null, "PDF", null },
                    { 2, null, null, null, "Video", null },
                    { 3, null, null, null, "WebsiteLink", null },
                    { 4, null, null, null, "QlikApplication", null },
                    { 5, null, null, null, "NativeTool", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details",
                column: "Project_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Type_Id",
                table: "Resource",
                column: "Type_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Category_Category_Id",
                table: "Resource_Category",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Category_Resource_Id_Category_Id",
                table: "Resource_Category",
                columns: new[] { "Resource_Id", "Category_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Technology_Resource_Id_Technology_Id",
                table: "Resource_Technology",
                columns: new[] { "Resource_Id", "Technology_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Technology_Technology_Id",
                table: "Resource_Technology",
                column: "Technology_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resource_Category");

            migrationBuilder.DropTable(
                name: "Resource_Technology");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Resource_Type");

            migrationBuilder.DropIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details");

            migrationBuilder.DropIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 4,
                column: "Permission_Name",
                value: "Dashboard Q3 Pricing Report");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 5,
                column: "Permission_Name",
                value: "Data Sync");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 6,
                column: "Permission_Name",
                value: "Event Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 7,
                column: "Permission_Name",
                value: "Messages All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 8,
                column: "Permission_Name",
                value: "Project Catalog View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 9,
                column: "Permission_Name",
                value: "Project Management All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 10,
                column: "Permission_Name",
                value: "Project Management Own");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 11,
                column: "Permission_Name",
                value: "Send Quote");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 12,
                column: "Permission_Name",
                value: "Tool Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 13,
                column: "Permission_Name",
                value: "Topic Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 14,
                column: "Permission_Name",
                value: "User Access Management");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Permission_Id",
                keyValue: 15,
                column: "Permission_Name",
                value: "User Profile Edit");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Permission_Id", "Created_User_Id", "Created_Ts", "Last_Change_Ts", "Permission_Name", "Updated_User_Id" },
                values: new object[] { 16, null, null, null, "Email Alert Management", null });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Renewable_Retail_Details_Project_Id",
                table: "Project_Renewable_Retail_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Onsite_Solar_Details_Project_Id",
                table: "Project_Onsite_Solar_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Offsite_Power_Purchase_Agreement_Details_Project_Id",
                table: "Project_Offsite_Power_Purchase_Agreement_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Green_Tariffs_Details_Project_Id",
                table: "Project_Green_Tariffs_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Fuel_Cells_Details_Project_Id",
                table: "Project_Fuel_Cells_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EV_Charging_Details_Project_Id",
                table: "Project_EV_Charging_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Emerging_Technology_Details_Project_Id",
                table: "Project_Emerging_Technology_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Equipment_Measures_Details_Project_Id",
                table: "Project_Efficiency_Equipment_Measures_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Efficiency_Audits_And_Consulting_Details_Project_Id",
                table: "Project_Efficiency_Audits_And_Consulting_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EAC_Details_Project_Id",
                table: "Project_EAC_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Community_Solar_Details_Project_Id",
                table: "Project_Community_Solar_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Carbon_Offsets_Details_Project_Id",
                table: "Project_Carbon_Offsets_Details",
                column: "Project_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Battery_Storage_Details_Project_Id",
                table: "Project_Battery_Storage_Details",
                column: "Project_Id");
        }
    }
}
